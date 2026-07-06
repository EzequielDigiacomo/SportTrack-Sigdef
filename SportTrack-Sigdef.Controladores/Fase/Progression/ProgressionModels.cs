using InscripcionEntity = SportTrack_Sigdef.Entidades.Entidades.Inscripcion;

namespace SportTrack_Sigdef.Controladores.Fase.Progression;

/// <summary>Asignación de un atleta a un destino con carril ICF.</summary>
public record ProgressionAssignment(
    InscripcionEntity Inscripcion,
    string Destino,   // SF1, SF2, SF3, SF4, FA, FB, FC
    int Carril,
    string Origen     // ej: "4/H1", "1st(7th)BT", "1/SF2"
);

public record SlotRule(int Heat, int Position, string Destino, int Carril);

/// <summary>BT: posición origen (8=8vo), rango entre iguales (1=mejor), destino y carril.</summary>
public record BtSlotRule(int SourcePosition, int BtRank, string Destino, int Carril);

public record RankedHeatContext(
    List<List<InscripcionEntity>> RankedByHeat,
    Func<InscripcionEntity, TimeSpan?> GetTime
);

public class PlanDefinition
{
    public required string PlanId { get; init; }
    public List<SlotRule> ElimToSemi { get; init; } = new();
    public List<BtSlotRule> ElimToSemiBt { get; init; } = new();
    public List<SlotRule> ElimToFinalA { get; init; } = new();
    public List<SlotRule> ElimToFinalB { get; init; } = new();
    public List<SlotRule> SemiToFinalA { get; init; } = new();
    public List<SlotRule> SemiToFinalB { get; init; } = new();
    public List<SlotRule> SemiToFinalC { get; init; } = new();
    public List<BtSlotRule> SemiToFinalBt { get; init; } = new();
}

public class ProgressionResult
{
    public Dictionary<string, Dictionary<int, InscripcionEntity>> Destinos { get; } = new();
    public List<ProgressionAssignment> AuditTrail { get; } = new();

    public void Assign(InscripcionEntity insc, string destino, int carril, string origen)
    {
        if (!Destinos.TryGetValue(destino, out var lanes))
        {
            lanes = new Dictionary<int, InscripcionEntity>();
            Destinos[destino] = lanes;
        }
        if (lanes.ContainsKey(carril))
            throw new InvalidOperationException($"Carril {carril} ya ocupado en {destino} (origen {origen}).");
        lanes[carril] = insc;
        AuditTrail.Add(new ProgressionAssignment(insc, destino, carril, origen));
    }

    public List<InscripcionEntity> GetInscripciones(string destino) =>
        Destinos.TryGetValue(destino, out var lanes)
            ? lanes.OrderBy(kv => kv.Key).Select(kv => kv.Value).ToList()
            : new List<InscripcionEntity>();
}
