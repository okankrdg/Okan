using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okan.IdentityServer.Models;

public class ClientDetail
{
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public string BaseAddress { get; set; } = "";
    public string LogOutAddress { get; set; } = "";
    public string Authority { get; set; } = "";
}

