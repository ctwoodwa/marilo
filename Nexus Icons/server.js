const express = require("express");
const fs = require("fs");
const path = require("path");

const app = express();
app.use(express.static("."));

// API endpoint to serve all icons as JSON
app.get("/api/icons", (req, res) => {
  const iconsDir = path.join(__dirname, "icons");
  const icons = [];

  const categories = fs.readdirSync(iconsDir).filter(f =>
    fs.statSync(path.join(iconsDir, f)).isDirectory()
  );

  for (const cat of categories) {
    const catDir = path.join(iconsDir, cat);
    const files = fs.readdirSync(catDir).filter(f => f.endsWith(".svg"));
    for (const file of files) {
      const svgContent = fs.readFileSync(path.join(catDir, file), "utf8");
      const name = file.replace(".svg", "");
      icons.push({
        name,
        category: cat,
        svg: svgContent,
        path: `icons/${cat}/${file}`
      });
    }
  }

  res.json(icons);
});

// Download endpoint for individual SVGs
app.get("/api/download/:category/:name", (req, res) => {
  const { category, name } = req.params;
  const filePath = path.join(__dirname, "icons", category, `${name}.svg`);
  if (fs.existsSync(filePath)) {
    res.setHeader("Content-Disposition", `attachment; filename="${name}.svg"`);
    res.setHeader("Content-Type", "image/svg+xml");
    res.sendFile(filePath);
  } else {
    res.status(404).json({ error: "Icon not found" });
  }
});

// Download all icons as a single JSON sprite map
app.get("/api/sprite", (req, res) => {
  const iconsDir = path.join(__dirname, "icons");
  const sprite = {};

  const categories = fs.readdirSync(iconsDir).filter(f =>
    fs.statSync(path.join(iconsDir, f)).isDirectory()
  );

  for (const cat of categories) {
    const catDir = path.join(iconsDir, cat);
    const files = fs.readdirSync(catDir).filter(f => f.endsWith(".svg"));
    for (const file of files) {
      const svgContent = fs.readFileSync(path.join(catDir, file), "utf8");
      const name = file.replace(".svg", "");
      sprite[`${cat}/${name}`] = svgContent;
    }
  }

  res.setHeader("Content-Disposition", 'attachment; filename="nexus-icons.json"');
  res.json(sprite);
});

app.get("/{*splat}", (req, res) => res.sendFile("index.html", { root: "." }));
app.listen(5000, "0.0.0.0", () => console.log("Nexus Icons → http://0.0.0.0:5000"));
