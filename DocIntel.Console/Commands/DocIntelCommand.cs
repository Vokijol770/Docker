// 

using System.Linq;

using DocIntel.Core.Models;
using DocIntel.Core.Repositories;
using DocIntel.Core.Settings;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;
using Spectre.Console.Cli;

namespace DocIntel.Console.Commands
{
    public abstract class DocIntelCommand<T> : AsyncCommand<T> where T : DocIntelCommandSettings
    {
        protected readonly ApplicationSettings _applicationSettings;
        protected readonly DocIntelContext _context;
        private readonly IUserClaimsPrincipalFactory<AppUser> _userClaimsPrincipalFactory;

        protected DocIntelCommand(DocIntelContext context,
            IUserClaimsPrincipalFactory<AppUser> userClaimsPrincipalFactory,
            ApplicationSettings applicationSettings)
        {
            _context = context;
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
            _applicationSettings = applicationSettings;
        }

        protected bool TryGetAmbientContext(out AmbientContext ambientContext)
        {
            var automationUser = GetAutomationUser();
            if (automationUser == null)
            {
                ambientContext = null;
                return false;
            }

            var claims = _userClaimsPrincipalFactory.CreateAsync(automationUser).Result;
            ambientContext = new AmbientContext
            {
                DatabaseContext = _context,
                Claims = claims,
                CurrentUser = automationUser
            };

            return true;
        }

        private AppUser GetAutomationUser()
        {
            var automationUser =
                _context.Users.AsNoTracking().FirstOrDefault(_ => _.UserName == _applicationSettings.AutomationAccount);
            if (automationUser == null)
                AnsiConsole.Render(
                    new Markup($"[red]The user '{_applicationSettings.AutomationAccount}' was not found.[/]"));

            return automationUser;
        }
    }
}