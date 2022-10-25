using Discord;
using Discord.Webhook;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RetroMikeMiningTools.Common;
using RetroMikeMiningTools.DAO;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace RetroMikeMiningTools.Pages
{
    [ResponseCache(NoStore = true, Duration = 0)]
    public class FeatureRequestModel : PageModel
    {
        [BindProperty]
        public string EnhancementDescription { get; set; }
        public string Message { get; set; }

        public async Task<IActionResult> OnPost(string description)
        {
            if (String.IsNullOrEmpty(description))
            {
                ViewData["Message"] = "Enhancement Writeup Required";
            }
            else
            {
                using (var client = new DiscordWebhookClient("https://discord.com/api/webhooks/1034249002566299730/QJna8JtFfkWd0sy4K9i35kQghepIwM91Ge1fWdEkQlVMX4hYrgCQRAk8cAGbbn5QWaYW"))
                {
                    var embed = new EmbedBuilder
                    {
                        Title = "New Enhancement Request",
                        Description = description
                    };

                    await client.SendMessageAsync(text: "New Enhancement Request", embeds: new[] { embed.Build() });
                }

                ViewData["Message"] = "Feature Request Submitted";
            }
            return Page();
        }
    }
}
