using DocIntel.Core.Utils.Features;
using Microsoft.Extensions.Logging;

namespace DocIntel.Tests;

public class TestExtractAttckTechnique
{
    [Test]
    public void TestExtractAttckTechniqueFromEsetReport()
    {
        var text = @"MITRE ATT&CK techniques
Tactic ID Name Description
Initial
Access
T1189 Drive-by Compromise Turla compromised high-value websites to
deliver malware to the visitors.
Execution T1204 User Execution A fake Flash installer is intended to trick the user
into launching the malware.
Persistence T1053 Scheduled Task NetFlash and PyFlash persist using scheduled
tasks.
Discovery T1016 System Network
Configuration Discovery
PyFlash executes ipconfig /all, getmac and arp -a
T1057 Process Discovery PyFlash executes tasklist
8/9 
T1082 System Information
Discovery
PyFlash executes systeminfo
Command
and
Control
T1032 Standard Cryptographic
Protocol
PyFlash uses AES-128 in CBC mode to encrypt
C&C communications.
T1043 Commonly Used Port NetFlash uses port 80.
T1065 Uncommonly Used Port PyFlash uses port 8,000.
A NetFlash sample uses port 15,363.
T1071 Standard Application
Layer Protocol
NetFlash and PyFlash use HTTP.
Exfiltration T1041 Exfiltration Over
Command and Control
Channel
The output of PyFlash surveillance and C&C
commands are exfiltrated using the C&C
protocol.";
        
        using var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.SetMinimumLevel(LogLevel.Trace);
            builder.AddConsole(options => options.DisableColors = true);
        });

        var extractor = new AttckTechniqueFeatureExtractor();
        var tlp = extractor.Extract(text);

        Console.WriteLine(string.Join(",", tlp));
        
        Assert.That(tlp.Count(), Is.EqualTo(11));
        Assert.That(tlp, Contains.Item("T1189"));
        Assert.That(tlp, Contains.Item("T1204"));
        Assert.That(tlp, Contains.Item("T1053"));
        Assert.That(tlp, Contains.Item("T1053"));
        Assert.That(tlp, Contains.Item("T1016"));
        Assert.That(tlp, Contains.Item("T1057"));
        Assert.That(tlp, Contains.Item("T1082"));
        Assert.That(tlp, Contains.Item("T1032"));
        Assert.That(tlp, Contains.Item("T1043"));
        Assert.That(tlp, Contains.Item("T1065"));
        Assert.That(tlp, Contains.Item("T1071"));
        Assert.That(tlp, Contains.Item("T1041"));
    }
}