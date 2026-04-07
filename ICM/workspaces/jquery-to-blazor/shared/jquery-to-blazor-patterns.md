# jQuery to Blazor Pattern Mapping

Quick-reference for converting jQuery constructs to their Blazor equivalents. Use during Stage 03 (component mapping) and Stage 04 (page conversion).

## Selectors and DOM Queries

| jQuery | Blazor Equivalent | Notes |
|--------|-------------------|-------|
| `$('#id')` | `@ref` directive on element | Use `ElementReference` for JSInterop only; prefer component state |
| `$('.class')` | No direct equivalent | Use component state and conditional rendering instead |
| `$('tag')` | No direct equivalent | Blazor does not query the DOM; manage elements via component tree |
| `$(this)` | Event handler parameter (`EventArgs`) | Access event target via `MouseEventArgs`, `ChangeEventArgs`, etc. |
| `$('#el').find('.child')` | Child component with `[Parameter]` | Use component composition instead of DOM traversal |

## Event Handling

| jQuery | Blazor Equivalent | Notes |
|--------|-------------------|-------|
| `$('#btn').click(fn)` | `<button @onclick="HandleClick">` | Direct event binding on element |
| `$('#el').on('click', '.child', fn)` | Event handler on child component | No event delegation; bind events directly on target elements |
| `$('#input').change(fn)` | `<input @onchange="HandleChange">` or `@bind` | Use `@bind` for two-way binding with state |
| `$('#input').keyup(fn)` | `<input @onkeyup="HandleKeyUp">` | All standard DOM events available via `@on[event]` |
| `$('#form').submit(fn)` | `<EditForm OnValidSubmit="HandleSubmit">` | Or `TelerikForm OnValidSubmit` |
| `$(document).ready(fn)` | `OnInitializedAsync()` lifecycle method | Runs when component is first initialized |
| `$(window).resize(fn)` | JSInterop with `window.addEventListener` | No native Blazor equivalent; use `TelerikMediaQuery` if possible |
| `$('#el').hover(fnIn, fnOut)` | `@onmouseenter` / `@onmouseleave` | Or CSS `:hover` for pure visual effects |
| `$('#el').focus(fn)` / `.blur(fn)` | `@onfocus` / `@onblur` | Available as Blazor DOM events |
| `$('#el').trigger('click')` | Call the handler method directly | Blazor calls methods, not DOM events |

## AJAX and Data

| jQuery | Blazor Equivalent | Notes |
|--------|-------------------|-------|
| `$.ajax({ url, method, data, success, error })` | `await HttpClient.SendAsync()` with try/catch | Register HttpClient via DI |
| `$.get(url, callback)` | `await HttpClient.GetFromJsonAsync<T>(url)` | Strongly typed response |
| `$.post(url, data, callback)` | `await HttpClient.PostAsJsonAsync(url, data)` | JSON serialization built in |
| `$.getJSON(url, callback)` | `await HttpClient.GetFromJsonAsync<T>(url)` | Same as $.get for JSON |
| `$.ajax` `beforeSend` callback | Set `isLoading = true` before await | Toggle loading state field |
| `$.ajax` `complete` callback | `finally` block after try/catch | Always runs regardless of success/error |
| `$.ajax` `error` callback | `catch (HttpRequestException ex)` | Handle in catch block |
| `$.ajax` `success` callback | Code after successful `await` | Runs on success only |
| `$.ajax` with `dataType: 'html'` | `await HttpClient.GetStringAsync(url)` | Then render with `@((MarkupString)html)` |

## DOM Manipulation

| jQuery | Blazor Equivalent | Notes |
|--------|-------------------|-------|
| `$('#el').show()` / `.hide()` | `@if (isVisible) { <div>...</div> }` | State-driven conditional rendering |
| `$('#el').toggle()` | `isVisible = !isVisible;` | Toggle boolean state |
| `$('#el').addClass('active')` | `class="@(isActive ? "active" : "")"` | Computed class string |
| `$('#el').removeClass('cls')` | Update state, re-render with new classes | No direct DOM class manipulation |
| `$('#el').toggleClass('cls')` | `isToggled = !isToggled;` + conditional class | State-driven class binding |
| `$('#el').css('color', 'red')` | `style="@($"color: {colorValue}")"` | Prefer CSS classes over inline styles |
| `$('#el').attr('disabled', true)` | `disabled="@isDisabled"` | Bind to component state |
| `$('#el').html(content)` | `@((MarkupString)content)` | Caution: raw HTML injection. Sanitize first |
| `$('#el').text(value)` | `@value` | Razor expression, auto-encoded |
| `$('#el').val(value)` | `@bind="fieldValue"` | Two-way binding on input |
| `$('#el').append(html)` | Add item to `List<T>`, re-render with `@foreach` | State-driven list rendering |
| `$('#el').remove()` | Remove item from collection, re-render | Component tree updates automatically |
| `$('#el').empty()` | Clear the collection or set content to null | Re-render handles DOM update |
| `$('#el').clone()` | Create new component instance | Blazor components are declarative |

## Animation and Effects

| jQuery | Blazor Equivalent | Notes |
|--------|-------------------|-------|
| `$('#el').fadeIn()` / `.fadeOut()` | CSS transitions + conditional rendering | Use `opacity` transition with `@if` |
| `$('#el').slideDown()` / `.slideUp()` | CSS transitions on `max-height` | Or use `TelerikAnimationContainer` |
| `$('#el').animate({})` | CSS `@keyframes` or `transition` | Blazor does not have a built-in animation API |
| `$('#el').delay(ms).fadeIn()` | `await Task.Delay(ms); isVisible = true;` | State change triggers re-render |

## Utilities

| jQuery | Blazor Equivalent | Notes |
|--------|-------------------|-------|
| `$.each(arr, fn)` | `foreach (var item in arr)` | C# iteration |
| `$.extend(obj1, obj2)` | Object spread or manual property copy | No direct equivalent; use record `with` |
| `$.trim(str)` | `str.Trim()` | Standard C# |
| `$.isArray(val)` | `val is Array` or `val is IEnumerable` | C# type checking |
| `$.inArray(val, arr)` | `arr.Contains(val)` or `Array.IndexOf` | LINQ or array methods |
| `$.parseJSON(str)` | `JsonSerializer.Deserialize<T>(str)` | System.Text.Json |
| `JSON.stringify(obj)` | `JsonSerializer.Serialize(obj)` | System.Text.Json |
| `setTimeout(fn, ms)` | `await Task.Delay(ms); fn();` | Or use `System.Timers.Timer` for recurring |
| `setInterval(fn, ms)` | `System.Timers.Timer` with `Elapsed` event | Dispose on component disposal |
| `clearTimeout` / `clearInterval` | `timer.Stop()` and `timer.Dispose()` | Implement `IDisposable` on the component |

## State Management

| jQuery | Blazor Equivalent | Notes |
|--------|-------------------|-------|
| Global variable | Scoped service via DI | Register as `AddScoped<T>()` |
| `data-*` attributes | `[Parameter]` on component | Or model properties |
| `$('#el').data('key', val)` | Component state field | Stored in C# memory, not DOM |
| `localStorage.getItem()` | `ProtectedLocalStorage` or JSInterop | Async in Blazor |
| `sessionStorage.getItem()` | `ProtectedSessionStorage` | Blazor Server only |
| Cookie read/write | `IJSRuntime` cookie access or middleware | No direct Blazor API |

## Navigation

| jQuery | Blazor Equivalent | Notes |
|--------|-------------------|-------|
| `window.location.href = url` | `NavigationManager.NavigateTo(url)` | Client-side navigation |
| `window.location.reload()` | `NavigationManager.NavigateTo(url, forceLoad: true)` | Full page reload |
| `window.history.pushState()` | Blazor routing handles this automatically | `@page` directive defines routes |
| `window.location.hash` | `NavigationManager.Uri` parsing | Extract fragment from URI |
| Query string parsing | `[SupplyParameterFromQuery]` attribute | .NET 8+ route parameter binding |
