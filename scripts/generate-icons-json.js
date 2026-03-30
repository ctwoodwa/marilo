#!/usr/bin/env node
/**
 * generate-icons-json.js
 *
 * Generates marilo-icons.json from sprite.svg + icon-categories.json.
 *
 * Single source of truth: src/Marilo.Icons/wwwroot/icons/marilo-icons.json
 * Copies distributed to:
 *   - samples/Marilo.Demo/wwwroot/icons/marilo-icons.json
 *   - samples/Marilo.Demo.FluentUI/wwwroot/icons/marilo-icons.json
 *   - docfx/marilo-icons.json  (served as static asset in DocFX _site)
 */

'use strict';

const fs = require('fs');
const path = require('path');

const ROOT = path.resolve(__dirname, '..');
const SPRITE   = path.join(ROOT, 'src/Marilo.Icons/wwwroot/icons/sprite.svg');
const CATS     = path.join(__dirname, 'icon-categories.json');
const PRIMARY  = path.join(ROOT, 'src/Marilo.Icons/wwwroot/icons/marilo-icons.json');

const COPIES = [
  path.join(ROOT, 'samples/Marilo.Demo/wwwroot/icons/marilo-icons.json'),
  path.join(ROOT, 'samples/Marilo.Demo.FluentUI/wwwroot/icons/marilo-icons.json'),
  path.join(ROOT, 'docfx/marilo-icons.json'),
];

// ── Parse sprite.svg ────────────────────────────────────────────────────────
const sprite = fs.readFileSync(SPRITE, 'utf8');
const icons  = {};

// Matches: <symbol id="marilo-NAME" ...>INNER CONTENT</symbol>
const symbolRe = /<symbol[^>]+\bid="marilo-([^"]+)"[^>]*>([\s\S]*?)<\/symbol>/g;
let m;
while ((m = symbolRe.exec(sprite)) !== null) {
  const name  = m[1];
  const inner = m[2].replace(/\n\s*/g, '').trim(); // collapse whitespace
  icons[name] = inner;
}

const count = Object.keys(icons).length;
if (count === 0) {
  console.error('ERROR: No icons found in sprite. Check the sprite format.');
  process.exit(1);
}

// ── Read categories ──────────────────────────────────────────────────────────
const categories = JSON.parse(fs.readFileSync(CATS, 'utf8'));

// Validate: warn about icon names in categories not found in sprite
let missing = 0;
for (const [cat, names] of Object.entries(categories)) {
  for (const name of names) {
    if (!icons[name]) {
      console.warn(`  WARN: "${name}" in category "${cat}" not found in sprite`);
      missing++;
    }
  }
}

// ── Write output ─────────────────────────────────────────────────────────────
const output = JSON.stringify({ icons, categories }, null, 2);

fs.writeFileSync(PRIMARY, output, 'utf8');
console.log(`✓ Generated ${count} icons → ${path.relative(ROOT, PRIMARY)}`);

if (missing > 0) {
  console.warn(`  ${missing} category entry/-ies not matched in sprite (listed above)`);
}

for (const dest of COPIES) {
  fs.mkdirSync(path.dirname(dest), { recursive: true });
  fs.writeFileSync(dest, output, 'utf8');
  console.log(`  → ${path.relative(ROOT, dest)}`);
}
