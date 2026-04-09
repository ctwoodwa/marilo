namespace Marilo.Components.Forms.Inputs;

/// <summary>
/// Interface that allows <see cref="ColorPickerViewBase"/> components to register
/// with their parent <see cref="MariloColorPicker"/> via cascading parameter.
/// </summary>
internal interface IColorPickerViewHost
{
    void RegisterView(ColorPickerViewBase view);
    void UnregisterView(ColorPickerViewBase view);
}
