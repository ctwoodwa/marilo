# VBScript to C# Conversion Reference

Side-by-side examples for converting VBScript (ASP Classic) to C# (Blazor). Use during Stage 04 page conversion.

## Variables and Types

| VBScript | C# | Notes |
|----------|-----|-------|
| `Dim name: name = "John"` | `string name = "John";` | Explicit types in C# |
| `Dim age: age = 30` | `int age = 30;` | |
| `Dim isActive: isActive = True` | `bool isActive = true;` | Lowercase `true`/`false` |

## String Operations

| VBScript | C# | Notes |
|----------|-----|-------|
| `Len(str)` | `str.Length` | |
| `Mid(str, start, len)` | `str.Substring(start - 1, len)` | VBScript is 1-based |
| `Left(str, n)` | `str[..n]` | Range operator |
| `Right(str, n)` | `str[^n..]` | Range operator |
| `InStr(str, find)` | `str.IndexOf(find) + 1` | Returns 0 if not found in VBS |
| `Replace(str, old, new)` | `str.Replace(old, new)` | |
| `UCase(str)` / `LCase(str)` | `str.ToUpper()` / `str.ToLower()` | |
| `Trim(str)` | `str.Trim()` | Also `TrimStart()`, `TrimEnd()` |
| `str1 & str2` | `$"{str1}{str2}"` | Prefer string interpolation |
| `Split(str, delim)` | `str.Split(delim)` | |
| `Join(arr, delim)` | `string.Join(delim, arr)` | |

## Date Operations

| VBScript | C# | Notes |
|----------|-----|-------|
| `Now` | `DateTime.Now` | |
| `Date` / `Time` | `DateTime.Today` / `DateTime.Now.TimeOfDay` | |
| `DateAdd("d", 5, dt)` | `dt.AddDays(5)` | Also `AddMonths`, `AddYears` |
| `DateDiff("d", dt1, dt2)` | `(dt2 - dt1).Days` | |
| `Year(dt)` / `Month(dt)` / `Day(dt)` | `dt.Year` / `dt.Month` / `dt.Day` | |
| `FormatDateTime(dt, 2)` | `dt.ToString("d")` | Use format strings |
| `CDate(str)` | `DateTime.Parse(str)` | Use `TryParse` for safety |
| `IsDate(val)` | `DateTime.TryParse(val, out _)` | |

## Type Conversion

| VBScript | C# | Notes |
|----------|-----|-------|
| `CInt(val)` / `CLng(val)` | `int.Parse(val)` / `long.Parse(val)` | |
| `CDbl(val)` | `double.Parse(val)` | |
| `CStr(val)` | `val.ToString()` | |
| `CBool(val)` | `Convert.ToBoolean(val)` | |
| `IsNumeric(val)` | `double.TryParse(val, out _)` | |
| `IsNull(val)` / `IsEmpty(val)` | `val is null` | |

## Control Flow

VBScript `If/ElseIf/End If` becomes C# `@if / else if / else` in Razor:

```csharp
@if (x > 10) { <span>big</span> }
else if (x > 5) { <span>medium</span> }
else { <span>small</span> }
```

VBScript `Select Case` becomes C# `switch`:

```csharp
var msg = status switch { "A" => "Active", "I" => "Inactive", _ => "Unknown" };
```

VBScript `For i = 1 To 10` becomes `@for (int i = 1; i <= 10; i++) { }`.
VBScript `For Each item In col` becomes `@foreach (var item in col) { }`.

## Error Handling

VBScript `On Error Resume Next` with `Err.Number` checks becomes `try/catch`:

```csharp
try {
    await using var conn = new SqlConnection(connString);
    await conn.OpenAsync();
}
catch (SqlException ex) {
    Logger.LogError(ex, "DB connection failed");
}
```

## Collections

| VBScript | C# | Notes |
|----------|-----|-------|
| `Scripting.Dictionary` | `Dictionary<string, object>` | |
| `dict.Add key, value` | `dict[key] = value` | |
| `dict.Exists(key)` | `dict.ContainsKey(key)` | |
| `Array(1, 2, 3)` | `new[] { 1, 2, 3 }` | |
| `UBound(arr)` | `arr.Length - 1` | |
| `ReDim Preserve arr(n)` | Use `List<T>` instead | Prefer `List<T>` |

## Common ASP Patterns

### Database query with recordset loop

```vbscript
' VBScript: manual connection, recordset loop, inline HTML
Set conn = Server.CreateObject("ADODB.Connection")
conn.Open Application("ConnString")
Set rs = conn.Execute("SELECT Name, Email FROM Users WHERE Active = 1")
Do While Not rs.EOF
    Response.Write "<tr><td>" & rs("Name") & "</td></tr>"
    rs.MoveNext
Loop
rs.Close: conn.Close
```

```csharp
// C# Blazor: injected service, TelerikGrid
@inject IUserService UserService
<TelerikGrid Data="@users" Pageable="true" Sortable="true">
    <GridColumns>
        <GridColumn Field="@nameof(User.Name)" />
        <GridColumn Field="@nameof(User.Email)" />
    </GridColumns>
</TelerikGrid>
@code {
    private List<User> users = new();
    protected override async Task OnInitializedAsync()
        => users = await UserService.GetActiveUsersAsync();
}
```

### Form submission

```vbscript
' VBScript: string-concatenated SQL (UNSAFE), manual redirect
name = Request.Form("name")
conn.Execute "INSERT INTO Users (Name) VALUES ('" & name & "')"
Response.Redirect "list.asp"
```

```csharp
// C# Blazor: Telerik Form, parameterized service, NavigationManager
@inject IUserService UserService
@inject NavigationManager Nav
<TelerikForm Model="@newUser" OnValidSubmit="@HandleSubmit">
    <FormItems>
        <FormItem Field="@nameof(User.Name)" />
    </FormItems>
</TelerikForm>
@code {
    private User newUser = new();
    private async Task HandleSubmit() {
        await UserService.CreateAsync(newUser);
        Nav.NavigateTo("/users");
    }
}
```
