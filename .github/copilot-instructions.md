# Workspace Instructions

## Project Overview

**SocOps** (Social Bingo) is an interactive web-based game built with **Blazor WebAssembly** and **.NET 10**. Players find people who match icebreaker prompts and mark squares to get 5 in a row, making it perfect for in-person mixers and corporate events.

**Tech Stack:**
- Frontend: Blazor WebAssembly components (C#)
- Backend: .NET 10 server
- State: LocalStorage via JSInterop
- Styling: Custom CSS utility classes (no external CSS framework)

## Architecture

```
SocOps/
├── Components/             # Blazor .razor components
│   ├── BingoBoard.razor    # Main game board display
│   ├── BingoSquare.razor   # Individual game square
│   ├── BingoModal.razor    # Win/result modal
│   ├── GameScreen.razor    # Active game view
│   └── StartScreen.razor   # Initial game setup
├── Pages/                  # Routable pages
│   ├── Home.razor          # Entry point
│   ├── Counter.razor       # Sample page
│   └── Weather.razor       # Sample page
├── Services/               # Business logic layers
│   ├── BingoGameService.cs    # State management, localStorage, events
│   └── BingoLogicService.cs   # Game rules (marking squares, win detection)
├── Models/                 # Data structures
│   ├── GameState.cs        # Current game state
│   ├── BingoSquareData.cs  # Square properties
│   └── BingoLine.cs        # Line detection
├── Data/                   # Static data
│   └── Questions.cs        # Icebreaker prompt collection
├── Layout/                 # Shell components
│   ├── MainLayout.razor
│   └── NavMenu.razor
└── wwwroot/                # Static assets
    ├── css/
    │   └── app.css         # Utility classes (see CSS utilities instruction)
    └── index.html          # Entry HTML
```

## Getting Started

### Prerequisites

- **.NET 10 SDK** (included in this dev container)
- **Node.js** (for any frontend tooling, optional)

### Key Commands

```bash
# Build the project
dotnet build SocOps/SocOps.csproj

# Run dev server with hot reload (port 5166)
dotnet run --project SocOps

# Run tests (when available)
dotnet test

# Clean build artifacts
dotnet clean SocOps/
```

### Running Locally

```bash
cd SocOps
dotnet run
# Server starts on http://localhost:5166
```

The dev server includes hot reload—edit files and see changes instantly without restarting.

## Development Patterns

### State Management

**BingoGameService** is the single source of truth:
- Manages `GameState` (current board, marked squares, winning lines)
- Persists to localStorage via `window.localStorage`
- Raises `OnStateChanged` event for component subscriptions
- Loads saved game on startup if available

**Components subscribe** like this:
```csharp
protected override void OnInitialized()
{
    BingoGameService.OnStateChanged += StateHasChanged;
}
```

### Game Logic

**BingoLogicService** handles rules-of-engagement:
- `MarkSquare()` – Toggle a square's marked state
- `CheckForWinner()` – Detect 5-in-a-row (horizontal, vertical, diagonal)
- Used by GameScreen and event handlers

### Component Patterns

Blazor components are `.razor` files with:
- **Template** (HTML-like markup)
- **Code** (@code block with C# logic)
- **Cascading parameters** for parent-to-child data
- **Event callbacks** for child-to-parent communication

Common structure:
```csharp
@page "/example"
@inject BingoGameService GameService

<div class="flex items-center">
    @foreach (var square in Squares)
    {
        <BingoSquare Data="square" OnClick="HandleClick" />
    }
</div>

@code {
    private async Task HandleClick(BingoSquareData square)
    {
        await GameService.MarkSquare(square.Id);
    }
}
```

## Styling

See [CSS utility classes instruction](.github/instructions/css-utilities.instructions.md) for detailed styling practices.

**Quick reference:**
- **Layout:** `.flex`, `.flex-col`, `.grid`, `.items-center`, `.justify-center`
- **Spacing:** `.p-2`, `.mb-4`, `.gap-3`, `.mx-auto`
- **Colors:** `.bg-accent` (blue), `.bg-marked` (green), `.text-gray-700`
- **Typography:** `.text-2xl`, `.font-bold`, `.font-semibold`
- **Sizing:** `.w-full`, `.h-full`, `.aspect-square`

**CSS Variables:**
```css
--color-accent: #0066be (primary blue)
--color-marked: #22c55e (marked green)
--color-border: #e5e7eb
```

## Code Conventions

### C# Style

- **Naming:** PascalCase for public members, camelCase for private
- **Nullability:** Enabled (`<Nullable>enable</Nullable>`). Use `?` for nullable types
- **Implicit Usings:** Enabled, reducing using statement clutter
- **Async:** Use `async`/`await` for DOM operations and services

Example:
```csharp
public class BingoGameService
{
    private GameState _gameState;
    
    public event Action? OnStateChanged;
    
    public async Task InitializeAsync()
    {
        // Load from localStorage
        _gameState = await LoadFromStorageAsync();
        NotifyStateChanged();
    }
    
    private void NotifyStateChanged() => OnStateChanged?.Invoke();
}
```

### Razor Components

- **Pascal case** file names (`BingoBoard.razor`, not `bingoBoard.razor`)
- **Props via parameters:** Use `[Parameter]` attributes
- **Events:** Use `EventCallback<T>` for bubbling data up
- **Disposal:** Implement `IAsyncDisposable` if subscribing to events

Example:
```razor
@implements IAsyncDisposable

@code {
    [Parameter]
    public BingoSquareData Data { get; set; }
    
    [Parameter]
    public EventCallback<BingoSquareData> OnClick { get; set; }
    
    protected override void OnInitialized()
    {
        BingoGameService.OnStateChanged += StateHasChanged;
    }
    
    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        BingoGameService.OnStateChanged -= StateHasChanged;
    }
}
```

## Front-End Design

When designing UI/UX, see [Frontend Design instruction](.github/instructions/frontend-design.instructions.md) for creating polished, distinctive components that avoid "AI slop" aesthetics.

Key principles:
- Choose distinctive typography (avoid generic Inter/Roboto)
- Commit to cohesive color themes with bold accents
- Use CSS animations for key moments (page load, win states)
- Match design complexity to aesthetic vision

## Testing & CI

Currently, no automated tests are configured. When adding tests:
- Use `xUnit` or `MSTest` (.NET standard)
- Place tests in `SocOps.Tests/` parallel to main project
- Run via `dotnet test`

## Common Development Tasks

### Adding a New Question Set

Edit `SocOps/Data/Questions.cs`:
```csharp
public static class Questions
{
    public static string[] TechMixerQuestions = new[]
    {
        "Has contributed to open source",
        "Works with machine learning",
        // Add more...
    };
}
```

### Creating a New Component

1. Create `SocOps/Components/MyComponent.razor`
2. Define `@code` with logic
3. Inject services as needed: `@inject BingoGameService GameService`
4. Use CSS utility classes for styling

### Debugging

- Open browser **DevTools** (F12) for JavaScript/DOM debugging
- Use **Blazor debugging** in VS Code (launch configs in `launch.json`)
- Check browser Console for JS errors, Network tab for API calls

## File Locations Reference

| Purpose | Location |
|---------|----------|
| Styling Practices | [`.github/instructions/css-utilities.instructions.md`](.github/instructions/css-utilities.instructions.md) |
| UI/Design Guidance | [`.github/instructions/frontend-design.instructions.md`](.github/instructions/frontend-design.instructions.md) |
| Game Questions | `SocOps/Data/Questions.cs` |
| Custom Agents | `.github/agents/` |
| Lab Workshop Guides | `workshop/` (5-part learning path) |

## Pre-Commit Checklist

Before pushing code:

- [ ] `dotnet build` passes (no errors or warnings)
- [ ] Components render without console errors
- [ ] Game state persists and loads correctly
- [ ] CSS utility classes used consistently
- [ ] No unused `@inject` directives or parameters
- [ ] Component event handlers properly wired

## Additional Resources

- **[Workshop Guides](../workshop/)** – 5-part learning lab on AI-assisted development
- **[README](../README.md)** – Project overview and quick links
- **[Microsoft Blazor Docs](https://learn.microsoft.com/aspnet/core/blazor/)** – Official documentation
- **[.NET 10 Release Notes](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-10/)** – Latest features
