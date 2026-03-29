/* ============================================================
   NEXUS ICONS — Interactive Preview App
   ============================================================ */

(function () {
  "use strict";

  // ── State ──
  let allIcons = [];
  let currentCategory = "all";
  let currentSize = 24;
  let currentSearch = "";
  let selectedIcon = null;

  // ── DOM Refs ──
  const grid = document.getElementById("iconGrid");
  const emptyState = document.getElementById("emptyState");
  const searchInput = document.getElementById("searchInput");
  const categoryTabs = document.querySelector(".category-tabs");
  const detailPanel = document.getElementById("detailPanel");
  const toast = document.getElementById("toast");

  // Category display names
  const categoryNames = {
    "ui-actions": "UI Actions",
    "navigation": "Navigation",
    "files": "Files & Documents",
    "media": "Media & Communication",
    "status": "Status & Indicators",
    "system": "System"
  };

  // Category order
  const categoryOrder = ["ui-actions", "navigation", "files", "media", "status", "system"];

  // ── Theme Toggle ──
  (function () {
    const toggle = document.querySelector("[data-theme-toggle]");
    const root = document.documentElement;
    let dark = matchMedia("(prefers-color-scheme:dark)").matches;
    root.setAttribute("data-theme", dark ? "dark" : "light");
    updateToggleIcon();

    toggle && toggle.addEventListener("click", () => {
      dark = !dark;
      root.setAttribute("data-theme", dark ? "dark" : "light");
      toggle.setAttribute("aria-label", "Switch to " + (dark ? "light" : "dark") + " mode");
      updateToggleIcon();
    });

    function updateToggleIcon() {
      if (!toggle) return;
      toggle.innerHTML = dark
        ? '<svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><circle cx="12" cy="12" r="5"/><path d="M12 1v2M12 21v2M4.22 4.22l1.42 1.42M18.36 18.36l1.42 1.42M1 12h2M21 12h2M4.22 19.78l1.42-1.42M18.36 5.64l1.42-1.42"/></svg>'
        : '<svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M21 12.79A9 9 0 1 1 11.21 3 7 7 0 0 0 21 12.79z"/></svg>';
    }
  })();

  // ── Load Icons ──
  async function loadIcons() {
    try {
      const res = await fetch("./api/icons");
      allIcons = await res.json();

      // Sort by category order then name
      allIcons.sort((a, b) => {
        const ai = categoryOrder.indexOf(a.category);
        const bi = categoryOrder.indexOf(b.category);
        if (ai !== bi) return ai - bi;
        return a.name.localeCompare(b.name);
      });

      buildCategoryTabs();
      renderGrid();
    } catch (err) {
      console.error("Failed to load icons:", err);
      grid.innerHTML = '<p style="color:var(--color-text-muted);padding:2rem;">Failed to load icons.</p>';
    }
  }

  // ── Category Tabs ──
  function buildCategoryTabs() {
    const counts = {};
    for (const icon of allIcons) {
      counts[icon.category] = (counts[icon.category] || 0) + 1;
    }

    let html = `<button class="cat-tab active" data-category="all" role="tab" aria-selected="true">All <span class="cat-count">${allIcons.length}</span></button>`;
    for (const cat of categoryOrder) {
      if (counts[cat]) {
        html += `<button class="cat-tab" data-category="${cat}" role="tab" aria-selected="false">${categoryNames[cat] || cat} <span class="cat-count">${counts[cat]}</span></button>`;
      }
    }
    categoryTabs.innerHTML = html;

    categoryTabs.addEventListener("click", (e) => {
      const tab = e.target.closest(".cat-tab");
      if (!tab) return;
      categoryTabs.querySelectorAll(".cat-tab").forEach(t => {
        t.classList.remove("active");
        t.setAttribute("aria-selected", "false");
      });
      tab.classList.add("active");
      tab.setAttribute("aria-selected", "true");
      currentCategory = tab.dataset.category;
      renderGrid();
    });
  }

  // ── Filter Icons ──
  function getFilteredIcons() {
    let filtered = allIcons;

    if (currentCategory !== "all") {
      filtered = filtered.filter(i => i.category === currentCategory);
    }

    if (currentSearch) {
      const q = currentSearch.toLowerCase();
      filtered = filtered.filter(i =>
        i.name.toLowerCase().includes(q) ||
        i.category.toLowerCase().includes(q) ||
        (categoryNames[i.category] || "").toLowerCase().includes(q)
      );
    }

    return filtered;
  }

  // ── Render Grid ──
  function renderGrid() {
    const filtered = getFilteredIcons();

    if (filtered.length === 0) {
      grid.innerHTML = "";
      emptyState.hidden = false;
      return;
    }

    emptyState.hidden = true;
    let html = "";
    let lastCategory = "";

    // Group by category when showing "All"
    const showHeaders = currentCategory === "all" && !currentSearch;

    for (let i = 0; i < filtered.length; i++) {
      const icon = filtered[i];

      if (showHeaders && icon.category !== lastCategory) {
        lastCategory = icon.category;
        const count = filtered.filter(ic => ic.category === icon.category).length;
        html += `<div class="category-header"><h3>${categoryNames[icon.category] || icon.category} <span class="cat-badge">${count}</span></h3></div>`;
      }

      // Inject SVG at the selected size
      const svgSized = icon.svg
        .replace(/width="24"/, `width="${currentSize}"`)
        .replace(/height="24"/, `height="${currentSize}"`);

      html += `<div class="icon-card" data-index="${allIcons.indexOf(icon)}" style="animation-delay:${Math.min(i * 15, 400)}ms">
        <div class="icon-preview">${svgSized}</div>
        <span class="icon-name">${icon.name}</span>
      </div>`;
    }

    grid.innerHTML = html;
  }

  // ── Search ──
  searchInput.addEventListener("input", (e) => {
    currentSearch = e.target.value.trim();
    renderGrid();
  });

  // Keyboard shortcut: / to focus search
  document.addEventListener("keydown", (e) => {
    if (e.key === "/" && document.activeElement !== searchInput) {
      e.preventDefault();
      searchInput.focus();
    }
    if (e.key === "Escape") {
      if (!detailPanel.hidden) {
        closeDetail();
      } else if (document.activeElement === searchInput) {
        searchInput.blur();
        searchInput.value = "";
        currentSearch = "";
        renderGrid();
      }
    }
  });

  // ── Size Controls ──
  document.querySelectorAll(".size-btn").forEach(btn => {
    btn.addEventListener("click", () => {
      document.querySelectorAll(".size-btn").forEach(b => b.classList.remove("active"));
      btn.classList.add("active");
      currentSize = parseInt(btn.dataset.size, 10);
      renderGrid();
    });
  });

  // ── Icon Click → Detail Panel ──
  grid.addEventListener("click", (e) => {
    const card = e.target.closest(".icon-card");
    if (!card) return;
    const idx = parseInt(card.dataset.index, 10);
    selectedIcon = allIcons[idx];
    openDetail();
  });

  function openDetail() {
    if (!selectedIcon) return;

    const preview = document.getElementById("detailPreview");
    const name = document.getElementById("detailName");
    const category = document.getElementById("detailCategory");
    const code = document.getElementById("detailCode");

    preview.innerHTML = selectedIcon.svg.replace(/width="24"/, 'width="48"').replace(/height="24"/, 'height="48"');
    name.textContent = selectedIcon.name;
    category.textContent = categoryNames[selectedIcon.category] || selectedIcon.category;

    // Size previews
    const sizes = [16, 24, 32, 48, 64];
    document.querySelectorAll(".detail-size-item").forEach((item) => {
      const s = parseInt(item.dataset.s, 10);
      const iconEl = item.querySelector(".detail-size-icon");
      iconEl.innerHTML = selectedIcon.svg
        .replace(/width="24"/, `width="${s}"`)
        .replace(/height="24"/, `height="${s}"`);
    });

    // Code
    code.textContent = formatSvg(selectedIcon.svg);

    detailPanel.hidden = false;
    document.body.style.overflow = "hidden";
  }

  function closeDetail() {
    detailPanel.hidden = true;
    document.body.style.overflow = "";
    selectedIcon = null;
  }

  // Backdrop click
  detailPanel.querySelector(".detail-backdrop").addEventListener("click", closeDetail);
  detailPanel.querySelector(".detail-close").addEventListener("click", closeDetail);

  // ── Copy / Download Actions ──
  document.getElementById("copySvgBtn").addEventListener("click", () => {
    if (!selectedIcon) return;
    copyToClipboard(selectedIcon.svg, "SVG copied");
  });

  document.getElementById("copyJsxBtn").addEventListener("click", () => {
    if (!selectedIcon) return;
    const jsx = svgToJsx(selectedIcon.svg);
    copyToClipboard(jsx, "JSX copied");
  });

  document.getElementById("downloadSvgBtn").addEventListener("click", () => {
    if (!selectedIcon) return;
    // Use the server download endpoint
    const a = document.createElement("a");
    a.href = `./api/download/${selectedIcon.category}/${selectedIcon.name}`;
    a.style.display = "none";
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);
    showToast("Download started");
  });

  document.getElementById("codeBlockCopy").addEventListener("click", () => {
    if (!selectedIcon) return;
    copyToClipboard(selectedIcon.svg, "Code copied");
  });

  document.getElementById("downloadAllBtn").addEventListener("click", () => {
    const a = document.createElement("a");
    a.href = "./api/sprite";
    a.style.display = "none";
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);
    showToast("Downloading icon sprite");
  });

  // ── Helpers ──
  function formatSvg(svg) {
    // Simple formatting: add newlines after tags
    return svg
      .replace(/>([^<])/g, ">\n  $1")
      .replace(/<\/(svg)>/g, "\n</$1>")
      .replace(/><((?!\/svg)[^>]+)/g, ">\n  <$1");
  }

  function svgToJsx(svg) {
    return svg
      .replace(/stroke-width/g, "strokeWidth")
      .replace(/stroke-linecap/g, "strokeLinecap")
      .replace(/stroke-linejoin/g, "strokeLinejoin")
      .replace(/fill-rule/g, "fillRule")
      .replace(/clip-rule/g, "clipRule")
      .replace(/xmlns="[^"]*"/g, "")
      .replace(/class="/g, 'className="');
  }

  async function copyToClipboard(text, message) {
    try {
      await navigator.clipboard.writeText(text);
      showToast(message || "Copied");
    } catch {
      // Fallback
      const textarea = document.createElement("textarea");
      textarea.value = text;
      textarea.style.position = "fixed";
      textarea.style.opacity = "0";
      document.body.appendChild(textarea);
      textarea.select();
      document.execCommand("copy");
      document.body.removeChild(textarea);
      showToast(message || "Copied");
    }
  }

  let toastTimer = null;
  function showToast(msg) {
    toast.textContent = msg;
    toast.classList.add("show");
    clearTimeout(toastTimer);
    toastTimer = setTimeout(() => toast.classList.remove("show"), 2000);
  }

  // ── Init ──
  loadIcons();

})();
