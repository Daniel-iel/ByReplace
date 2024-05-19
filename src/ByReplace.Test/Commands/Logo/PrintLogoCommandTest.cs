using ByReplace.Commands.Logo;
using ByReplace.Printers;
using Moq;
using Xunit;

namespace ByReplace.Test.Commands.Logo;

public class PrintLogoCommandTest
{
    private readonly Mock<IPrint> _print;

    public PrintLogoCommandTest()
    {
        _print = new Mock<IPrint>();
    }

    [Fact]
    public async Task Execute_WhenPrintByReplaceLogoInTerminal_ShouldValidateTheAsciiLogoWasPrintedOnce()
    {
        // Arrange
        const string expectedText = @"

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

        _print.Setup(c => c.PureText(expectedText));

        var command = new PrintLogoCommand(_print.Object);

        // Act
        await command.ExecuteAsync(It.IsAny<CancellationToken>());

        // Assert
        _print.Verify(c => c.PureText(expectedText), Times.Once);
    }
}
