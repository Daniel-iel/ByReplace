[assembly: InternalsVisibleTo("ByReplace.Test")]

namespace ByReplace.Commands.Logo;

internal class PrintLogoCommand : ICommand
{
    private readonly IPrint _print;

    public PrintLogoCommand(IPrint print)
    {
        _print = print;
    }

    public ValueTask ExecuteAsync(CancellationToken cancellationToken = default)
    {
        const string logoAscii = @"

                ,¿q▄▄▌██▓▓▓███████▌▄,              ,,,,,¿q▄▄▄▄▌▌▌▌▌▌▌▌▄▄µ
          ¿▄▌█▓▓▓▓▓▓████▒▀▀▀▀▀▀██▓▓▓█▌µ          ╒█▓▓▓▓▓▓▓▓▓▓▓▓█▒▒▒██▓▓▓▓▓█▄
       ¿▌▓▓▓▓▓▓▓▓▀▀╙             ²▓▓▓▓█           ▀▓▓▓▓▓╜             `▀▓▓▓▓▓µ
       Ñ▒▒█▓▓▓▓▓M                 █▓▓▓▓M          ]▓▓▓▓                  Ñ▓▓▓▓µ
           ██▓▓▌                 ▄▓▓▓▓▀           █▓▓▓▌                   ▓▓▓▓&
           ██▓▓&               ¿█▓▓▓█`           ]▓▓▓▓                   ]▓▓▓▓`
          ]██▓▓              ▄█▓▓▓▒╜             Ñ▓▓▓▌                  ▄█▓▓▓M
          ▐▓▓▓▌          ,q▌▓▓▓▓▀`               ▓▓▓▓M               ,▄▓▓▓▓▀
          █▓▓█M     ,¿▄▌███▓▓▓▓█▄µ              ▐▓▓▓▓            ,¿▄▓▓▓▓▓▀
         ]█▓▓█    ]█▓▓▓▓▓█▒▀▒██▒▓▓▒▌w           █▓▓▓▌   ç▄▄▄▄æg#███▒▓▒▀`
         ▐█▓▓▒     ╙╨╨╜`       `▀▒▓▓▓█µ         ▒▓▓▓M  Ñ▓▓▓▓▓████▀╜`
         ████&                    █████        ]█▒▒█    `╙╙▀██████▄
        ]████                     ▐████Ω       Ñ▒▒▒▌         `▀█████▌µ
        ▐███▌                     ████▌        Ñ▌▌▌▌            ╙▀▒▒ÅÑ▌▄µ
        Ñ▒██▀                   ¿Ñ██▒▀         ▌▀▀Ñ&               ╨▀▀Ñ▀▌▌▄µ
       ]▌ÑÑÑ[                ,q▌▄▐▒▒╜         ]▌▀▀▀&                 `╝Ñå▀Ñ█▒▄µ
       Ñ▌▌▌▌[             ¿q▀▌▌▌▌▌┘           ]▀Ñ▀▀&                    ╙▀▀▀▀▌▌▌&µ
       Ñ░▀▀▀M   ╓▄▄#╗æ#▀▌▒▒▀Ñ▀▀╨               ▀ååå▀                       ╙▀▀▀▀▀▌
        ╨▀▀╨   ▀É░▀▒@░░░░▀▌▀╙                  └▀▀╝`                          `╙`

";
        _print.PureText(logoAscii);
        //Console.WriteLine(logoAscii);

        return ValueTask.CompletedTask;
    }
}
