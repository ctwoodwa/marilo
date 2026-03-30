#!/usr/bin/env node
// Patches docfx files with URLs from site-links.json
// Run before docfx build: node scripts/patch-site-links.js

const fs = require('fs');
const path = require('path');

const root = path.resolve(__dirname, '..');
const links = JSON.parse(fs.readFileSync(path.join(root, 'site-links.json'), 'utf8'));
const demoUrl = links.demoBaseUrl;
const componentsUrl = links.componentsUrl || `${demoUrl}/components`;

// Patch toc.yml — Components nav link
const tocPath = path.join(root, 'docfx', 'toc.yml');
let toc = fs.readFileSync(tocPath, 'utf8');
toc = toc.replace(
    /^(- name: Components\n\s+href: ).*$/m,
    `$1${componentsUrl}`
);
fs.writeFileSync(tocPath, toc, 'utf8');

// Patch index.md — Components quick link
const indexPath = path.join(root, 'docfx', 'index.md');
let index = fs.readFileSync(indexPath, 'utf8');
index = index.replace(
    /\[Components\]\([^)]+\)/,
    `[Components](${componentsUrl})`
);
fs.writeFileSync(indexPath, index, 'utf8');

// Patch articles/icons.md — demo site icons link
const iconsPath = path.join(root, 'docfx', 'articles', 'icons.md');
let icons = fs.readFileSync(iconsPath, 'utf8');
icons = icons.replace(
    /\[Marilo demo site\]\([^)]+\)/,
    `[Marilo demo site](${demoUrl}/icons)`
);
fs.writeFileSync(iconsPath, icons, 'utf8');

console.log(`Patched site links: demoBaseUrl=${demoUrl}, componentsUrl=${componentsUrl}`);
