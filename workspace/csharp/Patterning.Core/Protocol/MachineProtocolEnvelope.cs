using System.Text.Json;

namespace Patterning.Core.Protocol;

/// <summary>Common JSON Lines protocol envelope for local TCP/IP components.</summary>
public sealed record MachineProtocolEnvelope(
    Guid MessageId,
    string MessageType,
    string SchemaVersion,
    DateTimeOffset SentAt,
    JsonElement Payload)
{
    /// <summary>Current proof-of-concept schema version.</summary>
    public const string CurrentSchemaVersion = "0.1";
}

/// <summary>Known message type names used by the simulator protocol.</summary>
public static class MachineMessageTypes
{
    public const string ConceptAnalyze = "concept.analyze";
    public const string ConceptAnalyzed = "concept.analyzed";
    public const string GridConvert = "grid.convert";
    public const string GridConverted = "grid.converted";
    public const string RunStart = "run.start";
    public const string RunPause = "run.pause";
    public const string RunResume = "run.resume";
    public const string RunReset = "run.reset";
    public const string ChannelActivate = "channel.activate";
    public const string StatusUpdate = "status.update";
    public const string DiagnosticRaised = "diagnostic.raised";
    public const string RunCompleted = "run.completed";
}
