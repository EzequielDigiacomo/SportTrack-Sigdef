using InscripcionEntity = SportTrack_Sigdef.Entidades.Entidades.Inscripcion;

namespace SportTrack_Sigdef.Controladores.Fase.Progression;

public static class ProgressionEngine
{
    public static ProgressionResult PromoteFromEliminatoria(PlanDefinition plan, RankedHeatContext ctx)
    {
        var result = new ProgressionResult();
        var used = new HashSet<int>();

        ApplySlots(plan.ElimToSemi, ctx, result, used, "H");
        ApplyBtSlots(plan.ElimToSemiBt, ctx, result, used, "BT");
        ApplySlots(plan.ElimToFinalA, ctx, result, used, "H");
        ApplySlots(plan.ElimToFinalB, ctx, result, used, "H");

        return result;
    }

    public static ProgressionResult PromoteFromSemifinal(PlanDefinition plan, RankedHeatContext ctx)
    {
        var result = new ProgressionResult();
        var used = new HashSet<int>();

        ApplySlots(plan.SemiToFinalA, ctx, result, used, "SF");
        ApplySlots(plan.SemiToFinalB, ctx, result, used, "SF");
        ApplySlots(plan.SemiToFinalC, ctx, result, used, "SF");
        ApplyBtSlots(plan.SemiToFinalBt, ctx, result, used, "BT");

        return result;
    }

    /// <summary>
    /// Reincorpora a Final A los pases directos desde eliminatorias (p. ej. 1° de cada serie en Plan B).
    /// Necesario al promover semifinales para completar los 9 carriles de FA.
    /// </summary>
    public static void AppendElimDirectToFinalA(
        PlanDefinition plan,
        RankedHeatContext elimCtx,
        ProgressionResult result)
    {
        if (!plan.ElimToFinalA.Any()) return;

        var used = new HashSet<int>(result.AuditTrail.Select(a => a.Inscripcion.IdInscripcion));
        ApplySlots(plan.ElimToFinalA, elimCtx, result, used, "H");
    }

    private static void ApplySlots(
        IEnumerable<SlotRule> rules,
        RankedHeatContext ctx,
        ProgressionResult result,
        HashSet<int> used,
        string heatPrefix)
    {
        foreach (var rule in rules)
        {
            var insc = GetAtPosition(ctx, rule.Heat, rule.Position);
            if (insc == null) continue;
            if (!used.Add(insc.IdInscripcion)) continue;

            var origen = $"{rule.Position}/{heatPrefix}{rule.Heat}";
            result.Assign(insc, rule.Destino, rule.Carril, origen);
        }
    }

    private static void ApplyBtSlots(
        IEnumerable<BtSlotRule> rules,
        RankedHeatContext ctx,
        ProgressionResult result,
        HashSet<int> used,
        string labelPrefix)
    {
        foreach (var group in rules.GroupBy(r => r.SourcePosition))
        {
            var pos = group.Key;
            var candidates = ctx.RankedByHeat
                .Select((heat, idx) =>
                {
                    if (heat.Count < pos) return null;
                    var insc = heat[pos - 1];
                    var time = ctx.GetTime(insc);
                    if (!time.HasValue) return null;
                    return new { Heat = idx + 1, Insc = insc, Time = time.Value };
                })
                .Where(x => x != null && !used.Contains(x!.Insc.IdInscripcion))
                .OrderBy(x => x!.Time)
                .ToList();

            foreach (var rule in group.OrderBy(r => r.BtRank))
            {
                var pick = candidates.ElementAtOrDefault(rule.BtRank - 1);
                if (pick == null) continue;
                if (!used.Add(pick.Insc.IdInscripcion)) continue;

                var rankLabel = rule.BtRank switch
                {
                    1 => "1st",
                    2 => "2nd",
                    3 => "3rd",
                    4 => "4th",
                    _ => $"{rule.BtRank}th"
                };
                var origen = $"{rankLabel}({pos}th){labelPrefix}";
                result.Assign(pick.Insc, rule.Destino, rule.Carril, origen);
            }
        }
    }

    private static InscripcionEntity? GetAtPosition(RankedHeatContext ctx, int heat, int position)
    {
        if (heat < 1 || heat > ctx.RankedByHeat.Count) return null;
        var list = ctx.RankedByHeat[heat - 1];
        if (position < 1 || position > list.Count) return null;
        return list[position - 1];
    }

    public static RankedHeatContext BuildContext(
        IEnumerable<Entidades.Entidades.Fase> fases,
        Func<Entidades.Entidades.Fase, IEnumerable<Entidades.Entidades.Resultado>> getResults)
    {
        var ranked = fases
            .OrderBy(f => f.NumeroFase)
            .Select(f => getResults(f)
                .Where(r => r.TiempoOficial.HasValue && r.Inscripcion != null)
                .OrderBy(r => r.TiempoOficial!.Value)
                .Select(r => r.Inscripcion!)
                .ToList())
            .ToList();

        var timeLookup = fases
            .SelectMany(f => getResults(f))
            .Where(r => r.Inscripcion != null && r.TiempoOficial.HasValue)
            .ToDictionary(r => r.InscripcionId, r => r.TiempoOficial);

        return new RankedHeatContext(
            ranked,
            insc => timeLookup.TryGetValue(insc.IdInscripcion, out var t) ? t : null
        );
    }

    public static string NormalizePlanId(string? planProgresion, int inscriptosCount)
    {
        if (!string.IsNullOrWhiteSpace(planProgresion))
        {
            var p = planProgresion.Trim().Replace(" ", "");
            if (p.StartsWith("Plan", StringComparison.OrdinalIgnoreCase))
                p = p[4..];
            if (ProgressionPlanRegistry.TryGet(p, out _)) return p;
        }
        return ProgressionPlanRegistry.ResolveDefaultPlan(inscriptosCount);
    }
}
