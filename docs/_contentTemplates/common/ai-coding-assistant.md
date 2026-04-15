#vs-intro
provides automated configuration commands for the Marilo AI-powered development tools. These commands help you quickly set up the [Marilo MCP server](slug:ai-overview) for enhanced developer productivity with Marilo UI for Blazor components.
#end

#prerequisites
* Check the tool-specific prerequisites for the [Marilo Blazor MCP Server](slug:agentic-ui-generator-getting-started).
#end

#verify-license-key
file to verify that the `MARILO_LICENSE_PATH` value matches your actual [Marilo license file location](slug:installation-license-key). Alternatively, replace `MARILO_LICENSE_PATH` with `MARILO_LICENSE` and set your license key directly. Using `MARILO_LICENSE_PATH` is recommended.
#end

#command-github-app
command opens the [MariloBlazor GitHub App installation page](https://github.com/apps/mariloblazor/installations/select_target) in your default browser.
#end

#copilot-instructions
command generates a `copilot-instructions.md` file in the `.github` folder under the solution. This file contains custom instructions that help GitHub Copilot provide better assistance when working with Marilo UI for Blazor components. The generated file includes the following default instructions:

* Guidance to use the Marilo MCP Server whenever applicable
* Guidance to prioritize the usage of Marilo UI components
* Guidance to use best coding practices related to Marilo UI for Blazor
#end
