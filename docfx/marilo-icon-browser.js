/*!
 * marilo-icon-browser.js
 * Self-contained icon browser widget for DocFX (and any static HTML page).
 *
 * Usage in Markdown / HTML:
 *   <div id="marilo-icon-browser"></div>
 *   <script>window.MARILO_ICONS_URL = '../marilo-icons.json';</script>
 *   <script src="../marilo-icon-browser.js"></script>
 */
(function () {
  'use strict';

  /* ── Config ──────────────────────────────────────────────────────── */
  const CONTAINER = '#marilo-icon-browser';
  const ICONS_URL = window.MARILO_ICONS_URL || '../marilo-icons.json';

  /* ── State ───────────────────────────────────────────────────────── */
  var state = {
    icons: {},
    categories: {},
    allNames: [],
    search: '',
    category: null,
    gridSize: 'm',
    selected: null,
    filtered: [],
    toastTimer: null,
  };

  /* ── Entry point ─────────────────────────────────────────────────── */
  function init() {
    var root = document.querySelector(CONTAINER);
    if (!root) return;

    injectStyles();
    root.innerHTML = '<div class="mib-loading">Loading icons…</div>';

    fetch(ICONS_URL)
      .then(function (r) { return r.json(); })
      .then(function (data) {
        state.icons = data.icons || {};
        state.categories = data.categories || {};
        state.allNames = Object.keys(state.icons);
        applyFilter();
        root.innerHTML = renderShell();
        bindEvents(root);
      })
      .catch(function (err) {
        root.innerHTML = '<p class="mib-error">Failed to load icons: ' + err.message + '</p>';
      });
  }

  /* ── Filtering ───────────────────────────────────────────────────── */
  function applyFilter() {
    var q = state.search.trim().toLowerCase();
    var source = state.category
      ? (state.categories[state.category] || []).filter(function (n) { return !!state.icons[n]; })
      : state.allNames;
    state.filtered = q
      ? source.filter(function (n) { return n.indexOf(q) !== -1; })
      : source;
  }

  /* ── Category label ──────────────────────────────────────────────── */
  function formatCat(cat) {
    if (!cat) return '';
    return cat.replace(/-/g, ' ').replace(/\b\w/g, function (c) { return c.toUpperCase(); });
  }

  /* ── Icon SVG ────────────────────────────────────────────────────── */
  function iconSvg(name, size) {
    var paths = state.icons[name];
    if (!paths) return '';
    return '<svg width="' + size + '" height="' + size + '" viewBox="0 0 24 24" fill="none"' +
      ' stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">' +
      paths + '</svg>';
  }

  function fullSvg(name) {
    var paths = state.icons[name];
    if (!paths) return '';
    return '<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"' +
      ' fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">' +
      paths + '</svg>';
  }

  function blazorSnippet(name) {
    return [
      '<MariloIcon Name="' + name + '" />',
      '',
      '<!-- with size -->',
      '<MariloIcon Name="' + name + '" Size="IconSize.Large" />',
      '',
      '<!-- with color -->',
      '<MariloIcon Name="' + name + '" ThemeColor="IconThemeColor.Primary" />',
    ].join('\n');
  }

  /* ── Rendering ───────────────────────────────────────────────────── */
  function renderShell() {
    var catCount = Object.keys(state.categories).length;
    var allCount = state.allNames.length;

    var catTabs = '<button class="mib-cat-tab' + (!state.category ? ' active' : '') +
      '" data-cat="">All <span class="mib-cat-count">' + allCount + '</span></button>';
    Object.keys(state.categories).forEach(function (cat) {
      var n = (state.categories[cat] || []).filter(function (name) { return !!state.icons[name]; }).length;
      if (!n) return;
      catTabs += '<button class="mib-cat-tab' + (state.category === cat ? ' active' : '') +
        '" data-cat="' + cat + '">' + formatCat(cat) + ' <span class="mib-cat-count">' + n + '</span></button>';
    });

    return [
      '<div class="mib-toolbar">',
      '  <input class="mib-search" type="search" placeholder="Search ' + allCount + ' icons… (press /)" value="' + esc(state.search) + '" autocomplete="off" />',
      '  <div class="mib-size-group" role="radiogroup" aria-label="Preview size">',
      ['s', 'm', 'l', 'xl'].map(function (k) {
        return '<button class="mib-size-btn' + (state.gridSize === k ? ' active' : '') +
          '" data-size="' + k + '" aria-pressed="' + (state.gridSize === k) + '">' + k.toUpperCase() + '</button>';
      }).join(''),
      '  </div>',
      '</div>',
      '<div class="mib-cat-row">' + catTabs + '</div>',
      '<div class="mib-grid-wrap">',
      renderGrid(),
      '</div>',
      '<div class="mib-detail" hidden></div>',
      '<div class="mib-toast" aria-live="polite"></div>',
    ].join('\n');
  }

  function renderGrid() {
    if (state.filtered.length === 0) {
      return '<div class="mib-empty">' +
        '<svg width="40" height="40" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"><circle cx="11" cy="11" r="8"/><path d="m21 21-4.35-4.35"/></svg>' +
        '<p>No icons match "' + esc(state.search) + '"</p></div>';
    }

    var showHeaders = !state.category && !state.search;
    var html = '<div class="mib-grid mib-grid--' + state.gridSize + '">';

    if (showHeaders) {
      Object.keys(state.categories).forEach(function (cat) {
        var names = (state.categories[cat] || []).filter(function (n) { return !!state.icons[n]; });
        if (!names.length) return;
        html += '<div class="mib-cat-header"><h3>' + formatCat(cat) +
          ' <span class="mib-cat-badge">' + names.length + '</span></h3></div>';
        names.forEach(function (name) {
          html += renderCard(name);
        });
      });
    } else {
      state.filtered.forEach(function (name) {
        html += renderCard(name);
      });
    }

    html += '</div>';
    return html;
  }

  function renderCard(name) {
    var sel = state.selected === name;
    return '<button class="mib-card' + (sel ? ' selected' : '') + '" data-icon="' + name + '" title="' + name + '">' +
      '<div class="mib-card-icon">' + iconSvg(name, 24) + '</div>' +
      '<span class="mib-card-name">' + name + '</span>' +
      '</button>';
  }

  function renderDetail(name) {
    var cat = '';
    Object.keys(state.categories).some(function (c) {
      if ((state.categories[c] || []).indexOf(name) !== -1) { cat = c; return true; }
    });
    return [
      '<div class="mib-detail-card">',
      '  <button class="mib-detail-close" aria-label="Close">',
      '    <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M18 6 6 18"/><path d="m6 6 12 12"/></svg>',
      '  </button>',
      '  <div class="mib-detail-preview">' + iconSvg(name, 48) + '</div>',
      '  <h3 class="mib-detail-name">' + name + '</h3>',
      '  <p class="mib-detail-cat">' + formatCat(cat) + '</p>',
      '  <div class="mib-detail-sizes">',
      [16, 24, 32, 48, 64].map(function (sz) {
        return '<div class="mib-size-item"><div class="mib-size-icon">' + iconSvg(name, sz) + '</div><span>' + sz + '</span></div>';
      }).join(''),
      '  </div>',
      '  <div class="mib-detail-actions">',
      '    <button class="mib-action-btn mib-action-btn--primary" data-action="copy-blazor">Copy Blazor</button>',
      '    <button class="mib-action-btn" data-action="copy-svg">Copy SVG</button>',
      '    <button class="mib-action-btn" data-action="download">Download</button>',
      '  </div>',
      '  <div class="mib-code-block">',
      '    <div class="mib-code-header"><span>Blazor</span>',
      '      <button class="mib-code-copy" data-action="copy-blazor" aria-label="Copy Blazor">',
      '        <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><rect width="14" height="14" x="8" y="8" rx="2"/><path d="M4 16c-1.1 0-2-.9-2-2V4c0-1.1.9-2 2-2h10c1.1 0 2 .9 2 2"/></svg>',
      '      </button>',
      '    </div>',
      '    <pre><code>' + esc(blazorSnippet(name)) + '</code></pre>',
      '  </div>',
      '  <div class="mib-code-block" style="margin-top:8px">',
      '    <div class="mib-code-header"><span>SVG</span>',
      '      <button class="mib-code-copy" data-action="copy-svg" aria-label="Copy SVG">',
      '        <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><rect width="14" height="14" x="8" y="8" rx="2"/><path d="M4 16c-1.1 0-2-.9-2-2V4c0-1.1.9-2 2-2h10c1.1 0 2 .9 2 2"/></svg>',
      '      </button>',
      '    </div>',
      '    <pre><code>' + esc(fullSvg(name)) + '</code></pre>',
      '  </div>',
      '</div>',
    ].join('\n');
  }

  /* ── Event binding ───────────────────────────────────────────────── */
  function bindEvents(root) {
    /* search */
    var searchEl = root.querySelector('.mib-search');
    searchEl.addEventListener('input', function () {
      state.search = this.value;
      applyFilter();
      refreshGrid(root);
    });

    /* '/' shortcut */
    document.addEventListener('keydown', function (e) {
      if (e.key === '/' && document.activeElement.tagName !== 'INPUT' &&
          document.activeElement.tagName !== 'TEXTAREA') {
        e.preventDefault();
        searchEl.focus();
      }
      if (e.key === 'Escape' && state.selected) {
        closeDetail(root);
      }
    });

    /* category tabs — delegated */
    root.addEventListener('click', function (e) {
      var catBtn = e.target.closest('.mib-cat-tab');
      if (catBtn) {
        state.category = catBtn.dataset.cat || null;
        applyFilter();
        root.querySelectorAll('.mib-cat-tab').forEach(function (b) {
          b.classList.toggle('active', b === catBtn);
        });
        refreshGrid(root);
        return;
      }

      /* size buttons */
      var sizeBtn = e.target.closest('.mib-size-btn');
      if (sizeBtn) {
        state.gridSize = sizeBtn.dataset.size;
        root.querySelectorAll('.mib-size-btn').forEach(function (b) {
          b.classList.toggle('active', b === sizeBtn);
          b.setAttribute('aria-pressed', b === sizeBtn);
        });
        refreshGrid(root);
        return;
      }

      /* icon cards */
      var card = e.target.closest('.mib-card');
      if (card) {
        var name = card.dataset.icon;
        if (state.selected === name) {
          closeDetail(root);
        } else {
          state.selected = name;
          root.querySelectorAll('.mib-card').forEach(function (c) {
            c.classList.toggle('selected', c.dataset.icon === name);
          });
          openDetail(root, name);
        }
        return;
      }

      /* detail close */
      if (e.target.closest('.mib-detail-close') || e.target.classList.contains('mib-detail')) {
        closeDetail(root);
        return;
      }

      /* action buttons */
      var actionEl = e.target.closest('[data-action]');
      if (actionEl && state.selected) {
        handleAction(root, actionEl.dataset.action);
        return;
      }
    });
  }

  function refreshGrid(root) {
    var wrap = root.querySelector('.mib-grid-wrap');
    if (wrap) wrap.innerHTML = renderGrid();
  }

  function openDetail(root, name) {
    var el = root.querySelector('.mib-detail');
    el.innerHTML = renderDetail(name);
    el.hidden = false;
  }

  function closeDetail(root) {
    state.selected = null;
    root.querySelectorAll('.mib-card.selected').forEach(function (c) { c.classList.remove('selected'); });
    var el = root.querySelector('.mib-detail');
    if (el) { el.hidden = true; el.innerHTML = ''; }
  }

  function handleAction(root, action) {
    var name = state.selected;
    if (!name) return;
    if (action === 'copy-svg') {
      copyText(fullSvg(name));
      showToast(root, 'SVG copied');
    } else if (action === 'copy-blazor') {
      copyText('<MariloIcon Name="' + name + '" />');
      showToast(root, 'Blazor markup copied');
    } else if (action === 'download') {
      var svg = fullSvg(name);
      var a = document.createElement('a');
      a.href = 'data:image/svg+xml,' + encodeURIComponent(svg);
      a.download = name + '.svg';
      a.click();
      showToast(root, 'Downloaded');
    }
  }

  function copyText(text) {
    if (navigator.clipboard && navigator.clipboard.writeText) {
      navigator.clipboard.writeText(text);
    } else {
      var ta = document.createElement('textarea');
      ta.value = text;
      ta.style.position = 'fixed';
      ta.style.opacity = '0';
      document.body.appendChild(ta);
      ta.select();
      document.execCommand('copy');
      document.body.removeChild(ta);
    }
  }

  function showToast(root, msg) {
    var el = root.querySelector('.mib-toast');
    if (!el) return;
    el.textContent = msg;
    el.classList.add('show');
    clearTimeout(state.toastTimer);
    state.toastTimer = setTimeout(function () { el.classList.remove('show'); }, 2200);
  }

  function esc(s) {
    return String(s)
      .replace(/&/g, '&amp;')
      .replace(/</g, '&lt;')
      .replace(/>/g, '&gt;')
      .replace(/"/g, '&quot;');
  }

  /* ── Styles ──────────────────────────────────────────────────────── */
  function injectStyles() {
    if (document.getElementById('mib-styles')) return;
    var style = document.createElement('style');
    style.id = 'mib-styles';
    style.textContent = [
      /* reset / layout */
      '#marilo-icon-browser *{box-sizing:border-box}',
      '#marilo-icon-browser{font-family:inherit;color:inherit}',

      /* toolbar */
      '.mib-toolbar{display:flex;align-items:center;gap:12px;padding:12px 0;flex-wrap:wrap}',
      '.mib-search{flex:1;min-width:180px;padding:7px 12px;border:1px solid var(--color-border,#dee2e6);border-radius:6px;font-size:.875rem;background:var(--color-surface,#fff);color:inherit;outline:none}',
      '.mib-search:focus{border-color:var(--color-primary,#0d6efd);box-shadow:0 0 0 3px rgba(13,110,253,.15)}',
      '.mib-size-group{display:flex;border:1px solid var(--color-border,#dee2e6);border-radius:6px;overflow:hidden}',
      '.mib-size-btn{padding:5px 11px;font-size:.6875rem;font-weight:600;background:var(--color-surface,#fff);border:none;border-right:1px solid var(--color-border,#dee2e6);cursor:pointer;color:var(--color-text-muted,#6c757d);font-family:inherit}',
      '.mib-size-btn:last-child{border-right:none}',
      '.mib-size-btn:hover{background:var(--color-bg-alt,#f5f5f5)}',
      '.mib-size-btn.active{background:var(--color-primary,#0d6efd);color:#fff}',

      /* category tabs */
      '.mib-cat-row{display:flex;gap:2px;overflow-x:auto;scrollbar-width:none;padding-bottom:10px}',
      '.mib-cat-row::-webkit-scrollbar{display:none}',
      '.mib-cat-tab{font-size:.8125rem;font-weight:500;color:var(--color-text-muted,#6c757d);padding:5px 12px;border-radius:6px;border:none;background:none;cursor:pointer;white-space:nowrap;display:inline-flex;align-items:center;gap:5px;font-family:inherit}',
      '.mib-cat-tab:hover{background:var(--color-bg-alt,#f5f5f5);color:var(--color-text,#212529)}',
      '.mib-cat-tab.active{background:var(--color-primary-soft,#e7f1ff);color:var(--color-primary,#0d6efd)}',
      '.mib-cat-count{font-size:.65rem;font-weight:600;opacity:.7}',

      /* grid */
      '.mib-grid{display:grid;gap:8px}',
      '.mib-grid--s{grid-template-columns:repeat(auto-fill,minmax(88px,1fr))}',
      '.mib-grid--m{grid-template-columns:repeat(auto-fill,minmax(110px,1fr))}',
      '.mib-grid--l{grid-template-columns:repeat(auto-fill,minmax(140px,1fr))}',
      '.mib-grid--xl{grid-template-columns:repeat(auto-fill,minmax(180px,1fr))}',

      /* category header in grid */
      '.mib-cat-header{grid-column:1/-1;padding:20px 0 6px;border-bottom:1px solid var(--color-divider,#e8e8e8);margin-bottom:4px}',
      '.mib-cat-header:first-child{padding-top:0}',
      '.mib-cat-header h3{font-size:.9375rem;font-weight:600;color:var(--color-text,#212529);margin:0;display:flex;align-items:center;gap:8px}',
      '.mib-cat-badge{font-size:.75rem;font-weight:400;color:var(--color-text-faint,#9e9e9e)}',

      /* cards */
      '.mib-card{display:flex;flex-direction:column;align-items:center;gap:7px;padding:14px 6px 10px;border-radius:8px;border:1px solid transparent;cursor:pointer;background:none;color:inherit;font-family:inherit;transition:background .15s,border-color .15s}',
      '.mib-card:hover{background:var(--color-bg-alt,#f5f5f5);border-color:var(--color-border,#dee2e6)}',
      '.mib-card.selected{background:var(--color-primary-soft,#e7f1ff);border-color:var(--color-primary,#0d6efd)}',
      '.mib-card.selected .mib-card-icon{color:var(--color-primary,#0d6efd)}',
      '.mib-card-icon{width:42px;height:42px;display:flex;align-items:center;justify-content:center;flex-shrink:0}',
      '.mib-card-name{font-size:.65rem;color:var(--color-text-muted,#6c757d);text-align:center;word-break:break-word;line-height:1.3;max-width:100%}',

      /* empty state */
      '.mib-empty{display:flex;flex-direction:column;align-items:center;gap:16px;padding:60px 24px;color:var(--color-text-faint,#9e9e9e);font-size:.9375rem}',
      '.mib-empty p{margin:0}',

      /* detail panel */
      '.mib-detail{position:fixed;inset:0;z-index:500;display:flex;align-items:center;justify-content:center;padding:24px;background:rgba(0,0,0,.4);backdrop-filter:blur(6px)}',
      '.mib-detail[hidden]{display:none}',
      '.mib-detail-card{position:relative;background:var(--color-surface,#fff);border:1px solid var(--color-border,#dee2e6);border-radius:14px;box-shadow:0 16px 48px rgba(0,0,0,.18);width:100%;max-width:460px;padding:28px;max-height:90vh;overflow-y:auto}',
      '.mib-detail-close{position:absolute;top:12px;right:12px;width:32px;height:32px;display:flex;align-items:center;justify-content:center;border-radius:6px;border:none;background:none;cursor:pointer;color:var(--color-text-muted,#6c757d)}',
      '.mib-detail-close:hover{background:var(--color-bg-alt,#f5f5f5)}',
      '.mib-detail-preview{display:flex;align-items:center;justify-content:center;width:88px;height:88px;margin:0 auto 18px;background:var(--color-bg-alt,#f5f5f5);border-radius:12px;color:var(--color-primary,#0d6efd)}',
      '.mib-detail-name{font-size:1.05rem;font-weight:600;text-align:center;margin:0 0 4px}',
      '.mib-detail-cat{text-align:center;font-size:.8125rem;color:var(--color-text-muted,#6c757d);margin:0 0 20px;text-transform:capitalize}',
      '.mib-detail-sizes{display:flex;align-items:flex-end;justify-content:center;gap:18px;padding:16px;background:var(--color-bg-alt,#f5f5f5);border-radius:8px;margin-bottom:16px;flex-wrap:wrap}',
      '.mib-size-item{display:flex;flex-direction:column;align-items:center;gap:6px}',
      '.mib-size-icon{display:flex;align-items:center;justify-content:center}',
      '.mib-size-item span{font-size:.6rem;color:var(--color-text-faint,#9e9e9e);font-weight:500}',

      /* action buttons */
      '.mib-detail-actions{display:flex;gap:6px;margin-bottom:16px}',
      '.mib-action-btn{flex:1;display:inline-flex;align-items:center;justify-content:center;gap:5px;padding:7px 10px;border-radius:6px;font-size:.78rem;font-weight:500;cursor:pointer;font-family:inherit;border:1px solid var(--color-border,#dee2e6);background:var(--color-surface,#fff);color:var(--color-text,#212529)}',
      '.mib-action-btn:hover{background:var(--color-bg-alt,#f5f5f5)}',
      '.mib-action-btn--primary{background:var(--color-primary,#0d6efd);border-color:var(--color-primary,#0d6efd);color:#fff}',
      '.mib-action-btn--primary:hover{background:var(--color-primary-hover,#0a58ca)}',

      /* code block */
      '.mib-code-block{background:var(--color-code-bg,#f5f5f5);border-radius:8px;overflow:hidden}',
      '.mib-code-header{display:flex;align-items:center;justify-content:space-between;padding:6px 12px;border-bottom:1px solid var(--color-divider,#e8e8e8);font-size:.65rem;font-weight:600;color:var(--color-text-muted,#6c757d);text-transform:uppercase;letter-spacing:.05em}',
      '.mib-code-copy{color:var(--color-text-faint,#9e9e9e);background:none;border:none;cursor:pointer;padding:3px;border-radius:4px;display:flex;align-items:center}',
      '.mib-code-copy:hover{color:var(--color-primary,#0d6efd)}',
      '.mib-code-block pre{padding:10px 12px;overflow-x:auto;font-family:"Cascadia Code","Fira Code",Consolas,monospace;font-size:.6875rem;line-height:1.6;color:var(--color-text-muted,#6c757d);white-space:pre-wrap;word-break:break-all;margin:0}',
      '.mib-code-block code{background:none;padding:0;font-size:inherit}',

      /* toast */
      '.mib-toast{position:fixed;bottom:28px;left:50%;transform:translateX(-50%) translateY(16px);background:var(--color-text,#212529);color:var(--color-bg,#fff);font-size:.8125rem;font-weight:500;padding:8px 18px;border-radius:999px;box-shadow:0 8px 24px rgba(0,0,0,.12);z-index:600;opacity:0;pointer-events:none;transition:opacity .2s,transform .2s;white-space:nowrap}',
      '.mib-toast.show{opacity:1;transform:translateX(-50%) translateY(0)}',

      /* loading / error */
      '.mib-loading,.mib-error{padding:40px;text-align:center;color:var(--color-text-muted,#6c757d)}',

      /* dark mode via .app-dark ancestor */
      '.app-dark .mib-search{background:var(--color-surface);border-color:var(--color-border)}',
      '.app-dark .mib-detail-card{background:var(--color-surface)}',
    ].join('\n');
    document.head.appendChild(style);
  }

  /* ── Bootstrap ───────────────────────────────────────────────────── */
  if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', init);
  } else {
    init();
  }
})();
