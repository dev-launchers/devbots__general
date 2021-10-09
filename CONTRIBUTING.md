# How to Contribute to DevBots

## Requirements:
Before you start contributing, you're going to need a few things:
- **Code Editor:**
    - We recommend [Visual Studio Code](https://code.visualstudio.com/) because of extensions and functionality with Unity.
      - Recommended Extensions:
      - Debugger for Unity
      - Unity Tools
      - Unity Code Snippets
    - Why? VS Code is familiar and tested by our team members, and is 100% guaranteed to support the useful functionality we appreciate.
- **Communication:**
    - While it may be obvious, you're going to need a GitHub account.
    - You're also going to need a Discord account, so you can:
        - Communicate effectively with other contributors
        - Test the changes you've made to the website
        - Receive help as you need it
    - Message `Quix (James)#9317` if confusion arrises!

# Standards

## Code Style

We use [C# coding conventions](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)

## Commits, Branches, and Pull Requests
- Before starting anything, make sure you:
    - ```bash
      git checkout master
      git pull
      git checkout -b "branch-name"
      ```
    - This ensures that you are working on the newest copy of the project.
- Make sure your commit messages are descriptive, and in the present tense (per convention)
    - Example (using active voice)
    ```
      // Very Bad:
      I created a card component and added the rainbow as well as updated the image size

      // Good:
      Create card component
        - add rainbow bar
        - update image size
    ```
    - Your commit message should look like a list of instructions
- Create a new branch for each new feature you add.
- You can create a pull request (commonly referred to as "PR") by clicking a little notice when you visit the repository that includes "merge changes"
- When creating a PR:
    - Give a summary of all the changes you've made
    - Explain the purpose for any new structures or systems you've created
    - Make a note of any other affected scripts or functionality
    - An example from our discord bot team is at [#101](https://github.com/dev-launchers-sandbox/project__discord-bot/pull/101)
    - The destination branch should be **main**
- During a PR:
    - You may be asked questions about what certain things do
    - You may be asked to refactor/change your code because:
        - It doesn't comply with the standards we've set forth
        - It would be wise to add a new feature
        - Or, something is missing or confusing

*(Template credit: Mohammed Maqbol Enjoy2Live)*
