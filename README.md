# Detective Roslyn IO

A gamified web portal to teach Roslyn SDK through progressive challenges.

## Overview

Detective Roslyn IO is an interactive learning platform where users can master the Roslyn SDK by solving progressively challenging tasks. Users work locally with Roslyn analyzers, code fixes, and source generators, then submit their answers through the web portal to track their progress.

## Features

- **Progressive Challenge System**: Challenges organized by difficulty (Beginner, Intermediate, Advanced) and category (Analyzer, Code Fix, Source Generator)
- **User Progress Tracking**: Track completion status, attempts, and hint usage
- **Admin Dashboard**: Easy challenge management through a web UI
- **Answer Validation**: Flexible answer validation supporting numbers, text, and comma-separated values
- **Hint System**: Progressive hints to help users when stuck
- **Authentication**: User registration and login with role-based access control

## Technology Stack

- **Framework**: ASP.NET Core 9.0 + Blazor Server
- **Database**: Entity Framework Core 9.0 with SQLite (dev) / SQL Server (prod)
- **Authentication**: ASP.NET Core Identity
- **UI**: Bootstrap 5

## Project Structure

```
Detective-Roslyn-IO/
├── DetectiveRoslynIO.sln
├── README.md
└── src/DetectiveRoslynIO/
    ├── Components/           # Blazor components
    │   ├── Layout/          # Layouts (Main, Admin)
    │   ├── Pages/           # Page components
    │   │   ├── Home.razor
    │   │   ├── Challenges/
    │   │   ├── Admin/
    │   │   └── Account/
    │   └── Shared/          # Reusable components
    ├── Data/                # Database context and entities
    │   ├── Entities/
    │   └── Configurations/
    ├── Services/            # Business logic
    ├── Models/              # View models and enums
    └── wwwroot/             # Static files
```

## Getting Started

### Prerequisites

- .NET 9.0 SDK
- SQLite (for development)

### Installation

1. Clone the repository:
   ```bash
   git clone <repository-url>
   cd Detective-Roslyn-IO
   ```

2. Restore dependencies:
   ```bash
   dotnet restore
   ```

3. Apply database migrations:
   ```bash
   cd src/DetectiveRoslynIO
   dotnet ef database update
   ```

4. Run the application:
   ```bash
   dotnet run
   ```

5. Open your browser and navigate to `http://localhost:5000`

### Default Admin Credentials

- **Email**: admin@detectiveroslyn.io
- **Password**: Admin123!

## Usage

### For Users

1. **Register**: Create an account on the portal
2. **Browse Challenges**: Explore available challenges filtered by category or difficulty
3. **Work Locally**: Clone the target repository and write your Roslyn code
4. **Submit Answers**: Return to the portal and submit your results
5. **Track Progress**: View your completion status and statistics

### For Admins

1. **Login** with admin credentials
2. **Access Admin Dashboard**: Navigate to `/admin`
3. **Create Challenges**: Use the "Create New Challenge" form
4. **Manage Challenges**: Edit or delete existing challenges
5. **View Submissions**: Monitor user submissions and progress

## Database Schema

### Challenge
- Challenge metadata (title, description, instructions)
- Category (Analyzer, CodeFix, SourceGenerator)
- Difficulty (Beginner, Intermediate, Advanced)
- Expected answer and validation rules
- Target repository URL

### Hint
- Associated with a challenge
- Ordered hints to progressively help users

### UserSubmission
- Tracks all answer attempts
- Records correctness and attempt number

### UserProgress
- Per-user, per-challenge progress
- Completion status, attempts, hints used

## Answer Validation

The portal supports three answer formats:

1. **Number**: Validates numerical answers (e.g., "7")
2. **Text**: Direct string comparison (case-sensitive option available)
3. **Comma-separated**: Validates lists of items (order-independent)

## Configuration

### Database Connection

Edit `appsettings.json` to configure the database:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=detectiveroslyn.db"
  }
}
```

For SQL Server:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=DetectiveRoslynIO;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

Then update `Program.cs` to use `UseSqlServer` instead of `UseSqlite`.

## Development

### Adding a New Challenge (via UI)

1. Login as admin
2. Navigate to Admin Dashboard
3. Click "Create New Challenge"
4. Fill in the form:
   - Title and description
   - Category and difficulty
   - Instructions (step-by-step guide)
   - Target repository URL
   - Expected answer and format
   - Hints (optional)
5. Save the challenge

### Running Migrations

```bash
# Create a new migration
dotnet ef migrations add MigrationName

# Apply migrations
dotnet ef database update

# Remove last migration (if not applied)
dotnet ef migrations remove
```

## Sample Challenges

The application seeds three sample challenges on first run:

1. **Find Unused Private Fields** (Beginner, Analyzer)
2. **Detect Empty Catch Blocks** (Beginner, Analyzer)
3. **Fix Naming Convention Violations** (Intermediate, Code Fix)

## Future Enhancements

- Leaderboard system
- Achievement badges
- Code upload for review
- Community discussions per challenge
- GitHub OAuth integration
- Progress export/certificates

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## License

This project is licensed under the MIT License.

## Support

For issues or questions:
- Create an issue in the GitHub repository
- Contact: admin@detectiveroslyn.io

---

**Built with ❤️ for the Roslyn SDK community**
