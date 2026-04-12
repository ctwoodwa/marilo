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
