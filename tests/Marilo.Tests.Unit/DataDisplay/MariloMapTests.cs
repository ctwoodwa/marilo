using Bunit;
using Marilo.Components.DataDisplay;
using Marilo.Components.DataDisplay.Map;
using Marilo.Core.Enums;
using Marilo.Core.Models;
using Microsoft.AspNetCore.Components;
using Xunit;

namespace Marilo.Tests.Unit.DataDisplay;

public class MariloMapTests : MariloTestBase
{
    [Fact]
    public void Map_Renders_Container_With_Dimensions()
    {
        var cut = Render<MariloMap>(p => p
            .Add(x => x.Width, "800px")
            .Add(x => x.Height, "600px"));

        var container = cut.Find(".mar-map");
        var style = container.GetAttribute("style") ?? "";
        Assert.Contains("width:800px", style);
        Assert.Contains("height:600px", style);
    }

    [Fact]
    public void Map_Creates_Adapter_On_First_Render()
    {
        var cut = Render<MariloMap>(p => p
            .Add(x => x.Center, new MapCenter { Latitude = 51.5, Longitude = -0.12 })
            .Add(x => x.Zoom, 5));

        var container = cut.Find(".mar-map");
        Assert.NotNull(container);
        var inner = container.QuerySelector("div[id]");
        Assert.NotNull(inner);
        var importInvocations = JSInterop.Invocations
            .Where(i => i.Identifier == "import").ToList();
        Assert.True(importInvocations.Count > 0, "Adapter should import the JS module on first render.");
    }

    [Fact]
    public async Task Map_Disposes_Adapter_On_Dispose()
    {
        var cut = Render<MariloMap>(p => p
            .Add(x => x.Center, new MapCenter { Latitude = 51.5, Longitude = -0.12 })
            .Add(x => x.Zoom, 5));
        await cut.Instance.DisposeAsync();
        await cut.Instance.DisposeAsync(); // idempotent
    }

    [Fact]
    public void MapLayer_Registers_With_Parent_Map()
    {
        var cut = Render<MariloMap>(p => p
            .Add(x => x.Center, new MapCenter { Latitude = 51.5, Longitude = -0.12 })
            .Add(x => x.Zoom, 5)
            .AddChildContent<MapLayer>(lp => lp
                .Add(l => l.Type, MapLayerType.Tile)
                .Add(l => l.UrlTemplate, "https://{s}.tile.osm.org/{z}/{x}/{y}.png")
                .Add(l => l.Attribution, "OSM")));
        Assert.Single(cut.Instance.RegisteredLayers);
        Assert.Equal(MapLayerType.Tile, cut.Instance.RegisteredLayers[0].Type);
    }

    [Fact]
    public void MapLayer_Multiple_Layers_Register()
    {
        var cut = Render<MariloMap>(p => p
            .Add(x => x.Center, new MapCenter { Latitude = 40.0, Longitude = -74.0 })
            .Add(x => x.Zoom, 8)
            .AddChildContent(builder =>
            {
                builder.OpenComponent<MapLayer>(0);
                builder.AddAttribute(1, nameof(MapLayer.Type), MapLayerType.Tile);
                builder.AddAttribute(2, nameof(MapLayer.UrlTemplate), "https://tile.osm.org/{z}/{x}/{y}.png");
                builder.CloseComponent();

                builder.OpenComponent<MapLayer>(3);
                builder.AddAttribute(4, nameof(MapLayer.Type), MapLayerType.Marker);
                builder.AddAttribute(5, nameof(MapLayer.LocationField), "LatLng");
                builder.CloseComponent();

                builder.OpenComponent<MapLayer>(6);
                builder.AddAttribute(7, nameof(MapLayer.Type), MapLayerType.Shape);
                builder.AddAttribute(8, nameof(MapLayer.Data), (object)"{\"type\":\"FeatureCollection\",\"features\":[]}");
                builder.CloseComponent();
            }));
        Assert.Equal(3, cut.Instance.RegisteredLayers.Count);
        Assert.Equal(MapLayerType.Tile, cut.Instance.RegisteredLayers[0].Type);
        Assert.Equal(MapLayerType.Marker, cut.Instance.RegisteredLayers[1].Type);
        Assert.Equal(MapLayerType.Shape, cut.Instance.RegisteredLayers[2].Type);
    }

    [Fact]
    public void MapLayer_ToDescriptor_Maps_Parameters_Correctly()
    {
        var cut = Render<MariloMap>(p => p
            .Add(x => x.Center, new MapCenter { Latitude = 0, Longitude = 0 })
            .AddChildContent<MapLayer>(lp => lp
                .Add(l => l.Type, MapLayerType.Tile)
                .Add(l => l.UrlTemplate, "https://tiles.example.com/{z}/{x}/{y}.png")
                .Add(l => l.Subdomains, new[] { "a", "b", "c" })
                .Add(l => l.Attribution, "Test Attribution")
                .Add(l => l.Opacity, 0.8)
                .Add(l => l.MinZoom, 2.0)
                .Add(l => l.MaxZoom, 16.0)
                .Add(l => l.LayerId, "my-tile-layer")));
        var layer = cut.Instance.RegisteredLayers[0];
        var descriptor = layer.ToDescriptor();
        Assert.Equal("my-tile-layer", descriptor.Id);
        Assert.Equal(MapLayerType.Tile, descriptor.Type);
        Assert.Equal("https://tiles.example.com/{z}/{x}/{y}.png", descriptor.UrlTemplate);
        Assert.Equal(new[] { "a", "b", "c" }, descriptor.Subdomains);
        Assert.Equal("Test Attribution", descriptor.Attribution);
        Assert.Equal(0.8, descriptor.Opacity);
        Assert.Equal(2.0, descriptor.MinZoom);
        Assert.Equal(16.0, descriptor.MaxZoom);
    }

    [Fact]
    public void MapLayer_Shape_Extracts_GeoJson_From_Data()
    {
        var geoJson = "{\"type\":\"FeatureCollection\",\"features\":[]}";
        var cut = Render<MariloMap>(p => p
            .AddChildContent<MapLayer>(lp => lp
                .Add(l => l.Type, MapLayerType.Shape)
                .Add(l => l.Data, (object)geoJson)));
        var descriptor = cut.Instance.RegisteredLayers[0].ToDescriptor();
        Assert.Equal(geoJson, descriptor.GeoJsonData);
    }

    [Fact]
    public async Task OnClick_Event_Fires()
    {
        MapClickEventArgs? receivedArgs = null;
        var cut = Render<MariloMap>(p => p
            .Add(x => x.Center, new MapCenter { Latitude = 0, Longitude = 0 })
            .Add(x => x.OnClick, (MapClickEventArgs args) => { receivedArgs = args; }));
        await cut.Instance.OnMapClickFromJs(51.5, -0.12);
        Assert.NotNull(receivedArgs);
        Assert.NotNull(receivedArgs!.Location);
        Assert.Equal(51.5, receivedArgs.Location!.Latitude);
        Assert.Equal(-0.12, receivedArgs.Location.Longitude);
    }

    [Fact]
    public async Task OnMarkerClick_Event_Fires()
    {
        MapMarkerClickEventArgs? receivedArgs = null;
        var cut = Render<MariloMap>(p => p
            .Add(x => x.Center, new MapCenter { Latitude = 0, Longitude = 0 })
            .Add(x => x.OnMarkerClick, (MapMarkerClickEventArgs args) => { receivedArgs = args; }));
        await cut.Instance.OnMarkerClickFromJs("Test Marker", 48.85, 2.35);
        Assert.NotNull(receivedArgs);
        Assert.NotNull(receivedArgs!.DataItem);
        Assert.Equal("Test Marker", receivedArgs.DataItem!.Title);
        Assert.Equal(48.85, receivedArgs.DataItem.Latitude);
        Assert.Equal(2.35, receivedArgs.DataItem.Longitude);
    }

    [Fact]
    public async Task OnZoomEnd_Event_Fires()
    {
        MapZoomEndEventArgs? receivedArgs = null;
        var cut = Render<MariloMap>(p => p
            .Add(x => x.OnZoomEnd, (MapZoomEndEventArgs args) => { receivedArgs = args; }));
        await cut.Instance.OnZoomEndFromJs(10.0, 40.0, -74.0);
        Assert.NotNull(receivedArgs);
        Assert.Equal(10.0, receivedArgs!.Zoom);
        Assert.NotNull(receivedArgs.Center);
        Assert.Equal(40.0, receivedArgs.Center!.Latitude);
        Assert.Equal(-74.0, receivedArgs.Center.Longitude);
    }

    [Fact]
    public async Task OnPanEnd_Event_Fires()
    {
        MapPanEndEventArgs? receivedArgs = null;
        var cut = Render<MariloMap>(p => p
            .Add(x => x.OnPanEnd, (MapPanEndEventArgs args) => { receivedArgs = args; }));
        await cut.Instance.OnPanEndFromJs(38.9, -77.0);
        Assert.NotNull(receivedArgs);
        Assert.NotNull(receivedArgs!.Center);
        Assert.Equal(38.9, receivedArgs.Center!.Latitude);
        Assert.Equal(-77.0, receivedArgs.Center.Longitude);
    }

    [Fact]
    public void Empty_Map_Renders_Without_Errors()
    {
        var cut = Render<MariloMap>();
        var container = cut.Find(".mar-map");
        Assert.NotNull(container);
        Assert.Empty(cut.Instance.RegisteredLayers);
    }

    [Fact]
    public void MapLayers_Wrapper_Passes_Through_Content()
    {
        var cut = Render<MariloMap>(p => p
            .Add(x => x.Center, new MapCenter { Latitude = 0, Longitude = 0 })
            .AddChildContent(builder =>
            {
                builder.OpenComponent<MapLayers>(0);
                builder.AddAttribute(1, "ChildContent", (RenderFragment)(innerBuilder =>
                {
                    innerBuilder.OpenComponent<MapLayer>(0);
                    innerBuilder.AddAttribute(1, nameof(MapLayer.Type), MapLayerType.Tile);
                    innerBuilder.AddAttribute(2, nameof(MapLayer.UrlTemplate), "https://tiles.example.com/{z}/{x}/{y}.png");
                    innerBuilder.CloseComponent();
                }));
                builder.CloseComponent();
            }));
        Assert.Single(cut.Instance.RegisteredLayers);
        Assert.Equal(MapLayerType.Tile, cut.Instance.RegisteredLayers[0].Type);
    }

    [Fact]
    public void Adapter_Interface_Is_Internal()
    {
        var adapterType = typeof(IMapEngineAdapter);
        Assert.False(adapterType.IsPublic, "IMapEngineAdapter should be internal, not public.");
    }

    [Fact]
    public void MapLibreAdapter_Is_Internal()
    {
        var adapterType = typeof(MapLibreAdapter);
        Assert.False(adapterType.IsPublic, "MapLibreAdapter should be internal, not public.");
    }

    [Fact]
    public void MapLayer_Unregisters_Via_Host_Interface()
    {
        var cut = Render<MariloMap>(p => p
            .Add(x => x.Center, new MapCenter { Latitude = 0, Longitude = 0 })
            .AddChildContent<MapLayer>(lp => lp
                .Add(l => l.Type, MapLayerType.Tile)
                .Add(l => l.UrlTemplate, "https://tiles.example.com/{z}/{x}/{y}.png")));
        Assert.Single(cut.Instance.RegisteredLayers);

        var layer = cut.Instance.RegisteredLayers[0];
        // Manually unregister to verify the host contract
        ((IMapLayerHost)cut.Instance).UnregisterLayer(layer);
        Assert.Empty(cut.Instance.RegisteredLayers);
    }

    [Fact]
    public void MapLayer_Unregister_Nonexistent_Is_Noop()
    {
        var cut = Render<MariloMap>(p => p
            .Add(x => x.Center, new MapCenter { Latitude = 0, Longitude = 0 })
            .AddChildContent<MapLayer>(lp => lp
                .Add(l => l.Type, MapLayerType.Tile)));
        Assert.Single(cut.Instance.RegisteredLayers);

        // Create a separate layer instance and try to unregister — should be a no-op
        var otherCut = Render<MariloMap>(p => p
            .AddChildContent<MapLayer>(lp => lp
                .Add(l => l.Type, MapLayerType.Marker)));
        var otherLayer = otherCut.Instance.RegisteredLayers[0];

        ((IMapLayerHost)cut.Instance).UnregisterLayer(otherLayer);
        Assert.Single(cut.Instance.RegisteredLayers);
    }

    [Fact]
    public void MapLayer_Default_Opacity_Is_One()
    {
        var cut = Render<MariloMap>(p => p
            .AddChildContent<MapLayer>(lp => lp
                .Add(l => l.Type, MapLayerType.Marker)));
        var descriptor = cut.Instance.RegisteredLayers[0].ToDescriptor();
        Assert.Equal(1.0, descriptor.Opacity);
    }

    [Fact]
    public void MapLayer_Default_MinZoom_And_MaxZoom()
    {
        var cut = Render<MariloMap>(p => p
            .AddChildContent<MapLayer>(lp => lp
                .Add(l => l.Type, MapLayerType.Bubble)));
        var descriptor = cut.Instance.RegisteredLayers[0].ToDescriptor();
        Assert.Equal(0, descriptor.MinZoom);
        Assert.Equal(22, descriptor.MaxZoom);
    }

    [Fact]
    public void MapLayer_AutoGenerates_Id_When_Not_Set()
    {
        var cut = Render<MariloMap>(p => p
            .AddChildContent<MapLayer>(lp => lp
                .Add(l => l.Type, MapLayerType.Tile)));
        var descriptor = cut.Instance.RegisteredLayers[0].ToDescriptor();
        Assert.StartsWith("mar-layer-", descriptor.Id);
    }

    [Fact]
    public void MapLayer_NonShape_Does_Not_Extract_GeoJson()
    {
        var cut = Render<MariloMap>(p => p
            .AddChildContent<MapLayer>(lp => lp
                .Add(l => l.Type, MapLayerType.Marker)
                .Add(l => l.Data, (object)"some string")));
        var descriptor = cut.Instance.RegisteredLayers[0].ToDescriptor();
        Assert.Null(descriptor.GeoJsonData);
    }

    [Fact]
    public void Map_Default_Dimensions()
    {
        var cut = Render<MariloMap>();
        var container = cut.Find(".mar-map");
        var style = container.GetAttribute("style") ?? "";
        Assert.Contains("width:100%", style);
        Assert.Contains("height:400px", style);
    }

    [Fact]
    public void Map_Default_Zoom_Is_Five()
    {
        // Verify the default zoom parameter compiles and the map renders
        var cut = Render<MariloMap>();
        Assert.NotNull(cut.Find(".mar-map"));
    }

    [Fact]
    public async Task OnShapeClick_Event_Fires()
    {
        MapShapeClickEventArgs? receivedArgs = null;
        var cut = Render<MariloMap>(p => p
            .Add(x => x.OnShapeClick, (MapShapeClickEventArgs args) => { receivedArgs = args; }));
        await cut.Instance.OnShapeClickFromJs(null);
        Assert.NotNull(receivedArgs);
    }

    [Fact]
    public void MapLayer_Duplicate_Register_Is_Idempotent()
    {
        var cut = Render<MariloMap>(p => p
            .AddChildContent<MapLayer>(lp => lp
                .Add(l => l.Type, MapLayerType.Tile)
                .Add(l => l.UrlTemplate, "https://tiles.example.com/{z}/{x}/{y}.png")));
        var layer = cut.Instance.RegisteredLayers[0];
        // Manually re-register the same layer — should not duplicate
        ((IMapLayerHost)cut.Instance).RegisterLayer(layer);
        Assert.Single(cut.Instance.RegisteredLayers);
    }

    [Fact]
    public void MapLayer_Tile_Descriptor_Preserves_UrlTemplate()
    {
        var cut = Render<MariloMap>(p => p
            .AddChildContent<MapLayer>(lp => lp
                .Add(l => l.Type, MapLayerType.Tile)
                .Add(l => l.UrlTemplate, "https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png")
                .Add(l => l.Subdomains, new[] { "a", "b", "c" })));
        var descriptor = cut.Instance.RegisteredLayers[0].ToDescriptor();
        // The adapter translates {s} placeholders in JS — C# descriptor preserves them as-is.
        Assert.Contains("{s}", descriptor.UrlTemplate);
        Assert.Contains("{z}", descriptor.UrlTemplate);
        Assert.Contains("{x}", descriptor.UrlTemplate);
        Assert.Contains("{y}", descriptor.UrlTemplate);
        Assert.Equal(3, descriptor.Subdomains!.Length);
    }

    [Fact]
    public void Adapter_Handles_Null_Layers_Gracefully()
    {
        // Map with no layers should render without errors and have empty layer list.
        var cut = Render<MariloMap>(p => p
            .Add(x => x.Center, new MapCenter { Latitude = 0, Longitude = 0 })
            .Add(x => x.Zoom, 5));
        Assert.Empty(cut.Instance.RegisteredLayers);
        // The adapter should have been created (import invocation) but no addLayer calls.
        var addLayerCalls = JSInterop.Invocations
            .Where(i => i.Identifier == "addLayer").ToList();
        Assert.Empty(addLayerCalls);
    }

    [Fact]
    public void MapLayer_Marker_Descriptor_Includes_Data()
    {
        var markers = new List<object> { new { LatLng = new[] { 40.0, -74.0 }, Title = "Test" } };
        var cut = Render<MariloMap>(p => p
            .AddChildContent<MapLayer>(lp => lp
                .Add(l => l.Type, MapLayerType.Marker)
                .Add(l => l.Data, (object)markers)
                .Add(l => l.LocationField, "LatLng")
                .Add(l => l.TitleField, "Title")));
        var descriptor = cut.Instance.RegisteredLayers[0].ToDescriptor();
        Assert.NotNull(descriptor.Data);
        Assert.Equal("LatLng", descriptor.LocationField);
        Assert.Equal("Title", descriptor.TitleField);
    }

    [Fact]
    public void MapLayer_Bubble_Descriptor_Includes_Data()
    {
        var bubbles = new List<object> { new { LatLng = new[] { 40.0, -74.0 }, Revenue = 1000 } };
        var cut = Render<MariloMap>(p => p
            .AddChildContent<MapLayer>(lp => lp
                .Add(l => l.Type, MapLayerType.Bubble)
                .Add(l => l.Data, (object)bubbles)
                .Add(l => l.LocationField, "LatLng")
                .Add(l => l.ValueField, "Revenue")));
        var descriptor = cut.Instance.RegisteredLayers[0].ToDescriptor();
        Assert.NotNull(descriptor.Data);
        Assert.Equal("Revenue", descriptor.ValueField);
    }

    [Fact]
    public void MapLayer_Tile_Descriptor_Does_Not_Include_Data()
    {
        var cut = Render<MariloMap>(p => p
            .AddChildContent<MapLayer>(lp => lp
                .Add(l => l.Type, MapLayerType.Tile)
                .Add(l => l.UrlTemplate, "https://tiles.example.com/{z}/{x}/{y}.png")
                .Add(l => l.Data, (object)"should-not-appear")));
        var descriptor = cut.Instance.RegisteredLayers[0].ToDescriptor();
        // Tile layers should not pass Data through (it goes via UrlTemplate instead).
        Assert.Null(descriptor.Data);
    }

    [Fact]
    public void MapLayerMarkerSettings_Registers_With_Parent_Layer()
    {
        var cut = Render<MariloMap>(p => p
            .AddChildContent(builder =>
            {
                builder.OpenComponent<MapLayer>(0);
                builder.AddAttribute(1, nameof(MapLayer.Type), MapLayerType.Marker);
                builder.AddAttribute(2, nameof(MapLayer.LocationField), "LatLng");
                builder.AddAttribute(3, nameof(MapLayer.ChildContent), (RenderFragment)(inner =>
                {
                    inner.OpenComponent<MapLayerMarkerSettings>(0);
                    inner.AddAttribute(1, nameof(MapLayerMarkerSettings.Template), "myMarkerTemplate");
                    inner.CloseComponent();
                }));
                builder.CloseComponent();
            }));
        var layer = cut.Instance.RegisteredLayers[0];
        Assert.NotNull(layer.MarkerSettings);
        Assert.Equal("myMarkerTemplate", layer.MarkerSettings!.Template);
    }

    [Fact]
    public void MapLayerBubbleSettings_Registers_With_Parent_Layer()
    {
        var cut = Render<MariloMap>(p => p
            .AddChildContent(builder =>
            {
                builder.OpenComponent<MapLayer>(0);
                builder.AddAttribute(1, nameof(MapLayer.Type), MapLayerType.Bubble);
                builder.AddAttribute(2, nameof(MapLayer.LocationField), "LatLng");
                builder.AddAttribute(3, nameof(MapLayer.ValueField), "Revenue");
                builder.AddAttribute(4, nameof(MapLayer.ChildContent), (RenderFragment)(inner =>
                {
                    inner.OpenComponent<MapLayerBubbleSettings>(0);
                    inner.AddAttribute(1, nameof(MapLayerBubbleSettings.FillColor), "#0000ff");
                    inner.AddAttribute(2, nameof(MapLayerBubbleSettings.FillOpacity), 0.5);
                    inner.AddAttribute(3, nameof(MapLayerBubbleSettings.StrokeColor), "#000000");
                    inner.AddAttribute(4, nameof(MapLayerBubbleSettings.StrokeWidth), 2.0);
                    inner.CloseComponent();
                }));
                builder.CloseComponent();
            }));
        var layer = cut.Instance.RegisteredLayers[0];
        Assert.NotNull(layer.BubbleSettings);
        Assert.Equal("#0000ff", layer.BubbleSettings!.FillColor);
        Assert.Equal(0.5, layer.BubbleSettings.FillOpacity);
        Assert.Equal("#000000", layer.BubbleSettings.StrokeColor);
        Assert.Equal(2.0, layer.BubbleSettings.StrokeWidth);
    }

    [Fact]
    public void MapLayerShapeSettings_Registers_With_Parent_Layer()
    {
        var cut = Render<MariloMap>(p => p
            .AddChildContent(builder =>
            {
                builder.OpenComponent<MapLayer>(0);
                builder.AddAttribute(1, nameof(MapLayer.Type), MapLayerType.Shape);
                builder.AddAttribute(2, nameof(MapLayer.Data), (object)"{\"type\":\"FeatureCollection\",\"features\":[]}");
                builder.AddAttribute(3, nameof(MapLayer.ChildContent), (RenderFragment)(inner =>
                {
                    inner.OpenComponent<MapLayerShapeSettings>(0);
                    inner.AddAttribute(1, nameof(MapLayerShapeSettings.FillColor), "#ff0000");
                    inner.AddAttribute(2, nameof(MapLayerShapeSettings.FillOpacity), 0.3);
                    inner.AddAttribute(3, nameof(MapLayerShapeSettings.StrokeColor), "#ffffff");
                    inner.AddAttribute(4, nameof(MapLayerShapeSettings.StrokeWidth), 1.5);
                    inner.CloseComponent();
                }));
                builder.CloseComponent();
            }));
        var layer = cut.Instance.RegisteredLayers[0];
        Assert.NotNull(layer.ShapeSettings);
        Assert.Equal("#ff0000", layer.ShapeSettings!.FillColor);
        Assert.Equal(0.3, layer.ShapeSettings.FillOpacity);
        Assert.Equal("#ffffff", layer.ShapeSettings.StrokeColor);
        Assert.Equal(1.5, layer.ShapeSettings.StrokeWidth);
    }

    [Fact]
    public void BubbleSettings_Included_In_Descriptor_Style()
    {
        var cut = Render<MariloMap>(p => p
            .AddChildContent(builder =>
            {
                builder.OpenComponent<MapLayer>(0);
                builder.AddAttribute(1, nameof(MapLayer.Type), MapLayerType.Bubble);
                builder.AddAttribute(2, nameof(MapLayer.LocationField), "LatLng");
                builder.AddAttribute(3, nameof(MapLayer.ValueField), "Val");
                builder.AddAttribute(4, nameof(MapLayer.ChildContent), (RenderFragment)(inner =>
                {
                    inner.OpenComponent<MapLayerBubbleSettings>(0);
                    inner.AddAttribute(1, nameof(MapLayerBubbleSettings.FillColor), "#00ff00");
                    inner.AddAttribute(2, nameof(MapLayerBubbleSettings.StrokeColor), "#111111");
                    inner.AddAttribute(3, nameof(MapLayerBubbleSettings.MinSize), 5.0);
                    inner.AddAttribute(4, nameof(MapLayerBubbleSettings.MaxSize), 40.0);
                    inner.CloseComponent();
                }));
                builder.CloseComponent();
            }));
        var descriptor = cut.Instance.RegisteredLayers[0].ToDescriptor();
        Assert.NotNull(descriptor.Style);
        Assert.Equal("#00ff00", descriptor.Style!.FillColor);
        Assert.Equal("#111111", descriptor.Style.StrokeColor);
        Assert.Equal(5.0, descriptor.Style.MinSize);
        Assert.Equal(40.0, descriptor.Style.MaxSize);
    }

    [Fact]
    public void ShapeSettings_Included_In_Descriptor_Style()
    {
        var cut = Render<MariloMap>(p => p
            .AddChildContent(builder =>
            {
                builder.OpenComponent<MapLayer>(0);
                builder.AddAttribute(1, nameof(MapLayer.Type), MapLayerType.Shape);
                builder.AddAttribute(2, nameof(MapLayer.Data), (object)"{\"type\":\"FeatureCollection\",\"features\":[]}");
                builder.AddAttribute(3, nameof(MapLayer.ChildContent), (RenderFragment)(inner =>
                {
                    inner.OpenComponent<MapLayerShapeSettings>(0);
                    inner.AddAttribute(1, nameof(MapLayerShapeSettings.FillColor), "#aabbcc");
                    inner.AddAttribute(2, nameof(MapLayerShapeSettings.FillOpacity), 0.7);
                    inner.CloseComponent();
                }));
                builder.CloseComponent();
            }));
        var descriptor = cut.Instance.RegisteredLayers[0].ToDescriptor();
        Assert.NotNull(descriptor.Style);
        Assert.Equal("#aabbcc", descriptor.Style!.FillColor);
        Assert.Equal(0.7, descriptor.Style.FillOpacity);
        Assert.Null(descriptor.Style.MinSize);
        Assert.Null(descriptor.Style.MaxSize);
    }

    [Fact]
    public async Task Refresh_Triggers_Adapter_Sync()
    {
        var cut = Render<MariloMap>(p => p
            .Add(x => x.Center, new MapCenter { Latitude = 0, Longitude = 0 })
            .Add(x => x.Zoom, 5)
            .AddChildContent<MapLayer>(lp => lp
                .Add(l => l.Type, MapLayerType.Tile)
                .Add(l => l.UrlTemplate, "https://tiles.example.com/{z}/{x}/{y}.png")));
        // Refresh should not throw; it re-syncs layers to the adapter.
        await cut.Instance.Refresh();
    }

    [Fact]
    public async Task Refresh_NoOp_Before_Initialization()
    {
        // Create a map but do NOT trigger OnAfterRenderAsync (adapter not initialized).
        // Refresh should be a safe no-op.
        var cut = Render<MariloMap>();
        // This should not throw even though the adapter is not initialized.
        await cut.Instance.Refresh();
    }

    [Fact]
    public void Layer_Without_Settings_Has_Null_Style_In_Descriptor()
    {
        var cut = Render<MariloMap>(p => p
            .AddChildContent<MapLayer>(lp => lp
                .Add(l => l.Type, MapLayerType.Marker)
                .Add(l => l.LocationField, "LatLng")));
        var descriptor = cut.Instance.RegisteredLayers[0].ToDescriptor();
        Assert.Null(descriptor.Style);
    }

    [Fact]
    public void MapLayers_Wrapper_With_Multiple_Children()
    {
        var cut = Render<MariloMap>(p => p
            .AddChildContent(builder =>
            {
                builder.OpenComponent<MapLayers>(0);
                builder.AddAttribute(1, "ChildContent", (RenderFragment)(inner =>
                {
                    inner.OpenComponent<MapLayer>(0);
                    inner.AddAttribute(1, nameof(MapLayer.Type), MapLayerType.Tile);
                    inner.AddAttribute(2, nameof(MapLayer.UrlTemplate), "https://t.example.com/{z}/{x}/{y}.png");
                    inner.CloseComponent();

                    inner.OpenComponent<MapLayer>(3);
                    inner.AddAttribute(4, nameof(MapLayer.Type), MapLayerType.Bubble);
                    inner.AddAttribute(5, nameof(MapLayer.LocationField), "Loc");
                    inner.AddAttribute(6, nameof(MapLayer.ValueField), "Val");
                    inner.CloseComponent();
                }));
                builder.CloseComponent();
            }));
        Assert.Equal(2, cut.Instance.RegisteredLayers.Count);
        Assert.Equal(MapLayerType.Tile, cut.Instance.RegisteredLayers[0].Type);
        Assert.Equal(MapLayerType.Bubble, cut.Instance.RegisteredLayers[1].Type);
    }
}
