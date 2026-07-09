namespace SportTrack_Sigdef.Controladores.Fase.Progression;

/// <summary>Reglas ICF Canoe Sprint — Planes A1-G2 según documentación oficial.</summary>
public static class ProgressionPlanRegistry
{
    private static readonly Dictionary<string, PlanDefinition> _plans = new(StringComparer.OrdinalIgnoreCase);

    static ProgressionPlanRegistry()
    {
        RegisterAll();
    }

    public static bool TryGet(string planId, out PlanDefinition plan) => _plans.TryGetValue(planId, out plan!);

    public static PlanDefinition Get(string planId) =>
        _plans.TryGetValue(planId, out var p) ? p : _plans["A1"];

    public static string ResolveDefaultPlan(int count)
    {
        var variant = Random.Shared.Next(1, 3);
        if (count >= 10 && count <= 18) return $"A{variant}";
        if (count >= 19 && count <= 27) return $"B{variant}";
        if (count >= 28 && count <= 36) return $"C{variant}";
        if (count >= 37 && count <= 45) return $"D{variant}";
        if (count >= 46 && count <= 54) return $"E{variant}";
        if (count >= 55 && count <= 63) return $"F{variant}";
        if (count >= 64 && count <= 72) return $"G{variant}";
        return "A1";
    }

    private static void RegisterAll()
    {
        // ─── PLAN A ───────────────────────────────────────────────
        Register(A1());
        Register(A2());

        // ─── PLAN B ───────────────────────────────────────────────
        Register(B1());
        Register(B2());

        // ─── PLAN C ───────────────────────────────────────────────
        Register(C1());
        Register(C2());

        // ─── PLAN D ───────────────────────────────────────────────
        Register(D1());
        Register(D2());

        // ─── PLAN E ───────────────────────────────────────────────
        Register(E1());
        Register(E2());

        // ─── PLAN F ───────────────────────────────────────────────
        Register(F1());
        Register(F2());

        // ─── PLAN G ───────────────────────────────────────────────
        Register(G1());
        Register(G2());

        foreach (var kv in _plans.ToList())
            _plans[$"Plan{kv.Key}"] = kv.Value;
    }

    private static void Register(PlanDefinition plan)
    {
        ValidatePlan(plan);
        _plans[plan.PlanId] = plan;
    }

    private static void ValidatePlan(PlanDefinition plan)
    {
        ValidateStaticSlots(plan, plan.ElimToSemi, "ElimToSemi");
        ValidateStaticSlots(plan, plan.ElimToFinalA, "ElimToFinalA");
        ValidateStaticSlots(plan, plan.ElimToFinalB, "ElimToFinalB");
        ValidateStaticSlots(plan, plan.ElimToSemi.Concat(plan.ElimToFinalA).Concat(plan.ElimToFinalB), "PromoteFromEliminatoria");
        ValidateStaticSlots(plan, plan.SemiToFinalA, "SemiToFinalA");
        ValidateStaticSlots(plan, plan.SemiToFinalB, "SemiToFinalB");
        ValidateStaticSlots(plan, plan.SemiToFinalC, "SemiToFinalC");
        ValidateStaticSlots(plan, plan.SemiToFinalA.Concat(plan.SemiToFinalB).Concat(plan.SemiToFinalC), "PromoteFromSemifinal");
    }

    private static void ValidateStaticSlots(PlanDefinition plan, IEnumerable<SlotRule> rules, string context)
    {
        var lanes = new Dictionary<string, HashSet<int>>(StringComparer.OrdinalIgnoreCase);
        foreach (var r in rules)
        {
            if (!lanes.TryGetValue(r.Destino, out var set))
            {
                set = new HashSet<int>();
                lanes[r.Destino] = set;
            }
            if (!set.Add(r.Carril))
            {
                throw new InvalidOperationException(
                    $"Plan {plan.PlanId} ({context}): carril {r.Carril} duplicado en {r.Destino} (H{r.Heat} P{r.Position})");
            }
        }
    }

    private static SlotRule S(int h, int p, string d, int l) => new(h, p, d, l);
    private static BtSlotRule B(int pos, int rank, string d, int l) => new(pos, rank, d, l);

    // ═══ PLAN A ═══════════════════════════════════════════════════
    private static PlanDefinition A1() => new()
    {
        PlanId = "A1",
        ElimToSemi =
        {
            S(1,4,"SF1",5), S(1,5,"SF1",6), S(1,6,"SF1",2), S(1,7,"SF1",8),
            S(2,4,"SF1",4), S(2,5,"SF1",3), S(2,6,"SF1",7), S(2,7,"SF1",1)
        },
        ElimToSemiBt = { B(8, 1, "SF1", 9) },
        ElimToFinalA =
        {
            S(1,1,"FA",5), S(1,2,"FA",3), S(1,3,"FA",7),
            S(2,1,"FA",4), S(2,2,"FA",6), S(2,3,"FA",2)
        },
        SemiToFinalA = { S(1,1,"FA",8), S(1,2,"FA",1), S(1,3,"FA",9) }
    };

    private static PlanDefinition A2() => new()
    {
        PlanId = "A2",
        ElimToSemi =
        {
            S(1,4,"SF1",4), S(1,5,"SF1",3), S(1,6,"SF1",7), S(1,7,"SF1",1),
            S(2,4,"SF1",5), S(2,5,"SF1",6), S(2,6,"SF1",2), S(2,7,"SF1",8)
        },
        ElimToSemiBt = { B(8, 1, "SF1", 9) },
        ElimToFinalA =
        {
            S(1,1,"FA",4), S(1,2,"FA",6), S(1,3,"FA",2),
            S(2,1,"FA",5), S(2,2,"FA",3), S(2,3,"FA",7)
        },
        SemiToFinalA = { S(1,1,"FA",1), S(1,2,"FA",9), S(1,3,"FA",8) }
    };

    // ═══ PLAN B ═══════════════════════════════════════════════════
    private static PlanDefinition B1() => new()
    {
        PlanId = "B1",
        ElimToFinalA = { S(1,1,"FA",5), S(2,1,"FA",4), S(3,1,"FA",6) },
        ElimToSemi =
        {
            S(1,2,"SF1",5), S(1,4,"SF1",7), S(1,6,"SF1",1),
            S(2,3,"SF1",4), S(2,5,"SF1",2), S(2,7,"SF1",9),
            S(3,3,"SF1",6), S(3,4,"SF1",3), S(3,6,"SF1",8),
            S(1,3,"SF2",6), S(1,5,"SF2",7), S(1,7,"SF2",1),
            S(2,2,"SF2",5), S(2,4,"SF2",3), S(2,6,"SF2",8),
            S(3,2,"SF2",4), S(3,5,"SF2",2), S(3,7,"SF2",9)
        },
        SemiToFinalA =
        {
            S(1,1,"FA",3), S(1,2,"FA",8), S(1,3,"FA",1),
            S(2,1,"FA",7), S(2,2,"FA",2), S(2,3,"FA",9)
        },
        SemiToFinalB =
        {
            S(1,4,"FB",5), S(1,5,"FB",3), S(1,6,"FB",7), S(1,7,"FB",1),
            S(2,4,"FB",4), S(2,5,"FB",6), S(2,6,"FB",2), S(2,7,"FB",8)
        },
        SemiToFinalBt = { B(8, 1, "FB", 9) }
    };

    private static PlanDefinition B2() => new()
    {
        PlanId = "B2",
        ElimToFinalA = { S(1,1,"FA",6), S(2,1,"FA",5), S(3,1,"FA",4) },
        ElimToSemi =
        {
            S(1,3,"SF1",6), S(1,5,"SF1",2), S(1,6,"SF1",9),
            S(2,3,"SF1",4), S(2,4,"SF1",7), S(2,6,"SF1",8),
            S(3,2,"SF1",5), S(3,4,"SF1",3), S(3,7,"SF1",1),
            S(1,2,"SF2",5), S(1,4,"SF2",3), S(1,7,"SF2",9),
            S(2,2,"SF2",4), S(2,5,"SF2",7), S(2,7,"SF2",1),
            S(3,3,"SF2",6), S(3,5,"SF2",2), S(3,6,"SF2",8)
        },
        SemiToFinalA =
        {
            S(1,1,"FA",7), S(1,2,"FA",2), S(1,3,"FA",9),
            S(2,1,"FA",3), S(2,2,"FA",8), S(2,3,"FA",1)
        },
        SemiToFinalB =
        {
            S(1,4,"FB",4), S(1,5,"FB",6), S(1,6,"FB",2), S(1,7,"FB",8),
            S(2,4,"FB",5), S(2,5,"FB",3), S(2,6,"FB",7), S(2,7,"FB",9)
        },
        SemiToFinalBt = { B(8, 1, "FB", 1) }
    };

    // ═══ PLAN C ═══════════════════════════════════════════════════
    private static PlanDefinition C1() => new()
    {
        PlanId = "C1",
        ElimToSemi =
        {
            S(1,1,"SF1",5), S(1,5,"SF1",8),
            S(2,2,"SF1",4), S(2,6,"SF1",1),
            S(3,2,"SF1",6), S(3,5,"SF1",2),
            S(4,3,"SF1",3), S(4,4,"SF1",7),
            S(1,3,"SF2",6), S(1,4,"SF2",2),
            S(2,1,"SF2",5), S(2,5,"SF2",8),
            S(3,3,"SF2",3), S(3,4,"SF2",7),
            S(4,2,"SF2",4), S(4,6,"SF2",9),
            S(1,2,"SF3",6), S(1,6,"SF3",1),
            S(2,3,"SF3",3), S(2,4,"SF3",7),
            S(3,1,"SF3",4), S(3,6,"SF3",8),
            S(4,1,"SF3",5), S(4,5,"SF3",2)
        },
        ElimToSemiBt = { B(7, 1, "SF1", 9), B(7, 2, "SF2", 1), B(7, 3, "SF3", 9) },
        SemiToFinalA =
        {
            S(1,1,"FA",5), S(1,2,"FA",3), S(1,3,"FA",8),
            S(2,1,"FA",4), S(2,2,"FA",7), S(2,3,"FA",1),
            S(3,1,"FA",6), S(3,2,"FA",2), S(3,3,"FA",9)
        },
        SemiToFinalB =
        {
            S(1,4,"FB",5), S(1,5,"FB",7), S(1,6,"FB",2),
            S(2,4,"FB",6), S(2,5,"FB",3), S(2,6,"FB",1),
            S(3,4,"FB",4), S(3,5,"FB",8), S(3,6,"FB",9)
        }
    };

    private static PlanDefinition C2() => new()
    {
        PlanId = "C2",
        ElimToSemi =
        {
            S(1,2,"SF1",4), S(1,6,"SF1",8),
            S(2,1,"SF1",6), S(2,6,"SF1",1),
            S(3,3,"SF1",3), S(3,4,"SF1",7),
            S(4,1,"SF1",5), S(4,5,"SF1",2),
            S(1,3,"SF2",6), S(1,5,"SF2",8),
            S(2,3,"SF2",3), S(2,4,"SF2",7),
            S(3,1,"SF2",5), S(3,6,"SF2",9),
            S(4,2,"SF2",4), S(4,4,"SF2",2),
            S(1,1,"SF3",5), S(1,4,"SF3",7),
            S(2,2,"SF3",6), S(2,5,"SF3",2),
            S(3,2,"SF3",4), S(3,5,"SF3",8),
            S(4,3,"SF3",3), S(4,6,"SF3",9)
        },
        ElimToSemiBt = { B(7, 1, "SF1", 9), B(7, 2, "SF2", 1), B(7, 3, "SF3", 1) },
        SemiToFinalA =
        {
            S(1,1,"FA",6), S(1,2,"FA",2), S(1,3,"FA",9),
            S(2,1,"FA",5), S(2,2,"FA",3), S(2,3,"FA",8),
            S(3,1,"FA",4), S(3,2,"FA",7), S(3,3,"FA",1)
        },
        SemiToFinalB =
        {
            S(1,4,"FB",4), S(1,5,"FB",8), S(1,6,"FB",9),
            S(2,4,"FB",5), S(2,5,"FB",7), S(2,6,"FB",2),
            S(3,4,"FB",6), S(3,5,"FB",3), S(3,6,"FB",1)
        }
    };

    // ═══ PLAN D ═══════════════════════════════════════════════════
    private static PlanDefinition D1() => new()
    {
        PlanId = "D1",
        ElimToSemi =
        {
            S(1,1,"SF1",5), S(1,4,"SF1",2),
            S(2,2,"SF1",6), S(2,5,"SF1",1),
            S(3,3,"SF1",7),
            S(4,1,"SF1",4), S(4,4,"SF1",8),
            S(5,2,"SF1",3), S(5,5,"SF1",9),
            S(1,2,"SF2",4), S(1,5,"SF2",8),
            S(2,3,"SF2",3),
            S(3,1,"SF2",5), S(3,4,"SF2",2),
            S(4,2,"SF2",6), S(4,5,"SF2",1),
            S(5,3,"SF2",7),
            S(1,3,"SF3",3),
            S(2,1,"SF3",5), S(2,4,"SF3",2),
            S(3,2,"SF3",6), S(3,5,"SF3",1),
            S(4,3,"SF3",7),
            S(5,1,"SF3",4), S(5,4,"SF3",8)
        },
        ElimToSemiBt = { B(6, 1, "SF2", 9), B(6, 2, "SF3", 9) },
        SemiToFinalA =
        {
            S(1,1,"FA",5), S(1,2,"FA",3), S(1,3,"FA",8),
            S(2,1,"FA",4), S(2,2,"FA",7), S(2,3,"FA",1),
            S(3,1,"FA",6), S(3,2,"FA",2), S(3,3,"FA",9)
        },
        SemiToFinalB =
        {
            S(1,4,"FB",5), S(1,5,"FB",7), S(1,6,"FB",2),
            S(2,4,"FB",6), S(2,5,"FB",3), S(2,6,"FB",1),
            S(3,4,"FB",4), S(3,5,"FB",8), S(3,6,"FB",9)
        },
        SemiToFinalC =
        {
            S(1,7,"FC",5), S(1,8,"FC",3), S(1,9,"FC",8),
            S(2,7,"FC",4), S(2,8,"FC",7), S(2,9,"FC",1),
            S(3,7,"FC",6), S(3,8,"FC",2), S(3,9,"FC",9)
        }
    };

    private static PlanDefinition D2() => new()
    {
        PlanId = "D2",
        ElimToSemi =
        {
            S(1,2,"SF1",6), S(1,4,"SF1",2),
            S(2,1,"SF1",5), S(2,5,"SF1",9),
            S(3,2,"SF1",3),
            S(4,3,"SF1",7), S(4,4,"SF1",8),
            S(5,1,"SF1",4), S(5,4,"SF1",1),
            S(1,1,"SF2",5), S(1,5,"SF2",2),
            S(2,4,"SF2",7),
            S(3,3,"SF2",3), S(3,5,"SF2",8),
            S(4,2,"SF2",4),
            S(5,2,"SF2",6), S(5,5,"SF2",1),
            S(1,3,"SF3",3),
            S(2,2,"SF3",6), S(2,3,"SF3",7),
            S(3,1,"SF3",5), S(3,4,"SF3",8),
            S(4,1,"SF3",4), S(4,5,"SF3",1),
            S(5,3,"SF3",2)
        },
        ElimToSemiBt = { B(6, 1, "SF3", 9), B(6, 2, "SF2", 9) },
        SemiToFinalA =
        {
            S(1,1,"FA",6), S(1,2,"FA",2), S(1,3,"FA",9),
            S(2,1,"FA",4), S(2,2,"FA",3), S(2,3,"FA",8),
            S(3,1,"FA",5), S(3,2,"FA",7), S(3,3,"FA",1)
        },
        SemiToFinalB =
        {
            S(1,4,"FB",4), S(1,5,"FB",8), S(1,6,"FB",9),
            S(2,4,"FB",5), S(2,5,"FB",7), S(2,6,"FB",1),
            S(3,4,"FB",6), S(3,5,"FB",3), S(3,6,"FB",2)
        },
        SemiToFinalC =
        {
            S(1,7,"FC",6), S(1,8,"FC",2), S(1,9,"FC",9),
            S(2,7,"FC",5), S(2,8,"FC",3), S(2,9,"FC",8),
            S(3,7,"FC",4), S(3,8,"FC",7), S(3,9,"FC",1)
        }
    };

    // ═══ PLAN E ═══════════════════════════════════════════════════
    private static PlanDefinition E1() => new()
    {
        PlanId = "E1",
        ElimToSemi =
        {
            S(1,1,"SF1",5), S(1,4,"SF1",8),
            S(2,3,"SF1",7),
            S(3,2,"SF1",3),
            S(4,1,"SF1",4), S(4,4,"SF1",1),
            S(5,3,"SF1",2),
            S(6,2,"SF1",6),
            S(1,2,"SF2",3),
            S(2,1,"SF2",5), S(2,4,"SF2",8),
            S(3,3,"SF2",7),
            S(4,2,"SF2",6),
            S(5,1,"SF2",4), S(5,4,"SF2",1),
            S(6,3,"SF2",2),
            S(1,3,"SF3",7),
            S(2,2,"SF3",3),
            S(3,1,"SF3",5), S(3,4,"SF3",8),
            S(4,3,"SF3",2),
            S(5,2,"SF3",6),
            S(6,1,"SF3",4), S(6,4,"SF3",1)
        },
        ElimToSemiBt = { B(5, 1, "SF1", 9), B(5, 2, "SF2", 9), B(5, 3, "SF3", 9) },
        SemiToFinalA =
        {
            S(1,1,"FA",5), S(1,2,"FA",3), S(1,3,"FA",8),
            S(2,1,"FA",4), S(2,2,"FA",7), S(2,3,"FA",1),
            S(3,1,"FA",6), S(3,2,"FA",2), S(3,3,"FA",9)
        },
        SemiToFinalB =
        {
            S(1,4,"FB",5), S(1,5,"FB",7), S(1,6,"FB",2),
            S(2,4,"FB",6), S(2,5,"FB",3), S(2,6,"FB",1),
            S(3,4,"FB",4), S(3,5,"FB",8), S(3,6,"FB",9)
        },
        SemiToFinalC =
        {
            S(1,7,"FC",5), S(1,8,"FC",3), S(1,9,"FC",8),
            S(2,7,"FC",4), S(2,8,"FC",7), S(2,9,"FC",1),
            S(3,7,"FC",6), S(3,8,"FC",2), S(3,9,"FC",9)
        }
    };

    private static PlanDefinition E2() => new()
    {
        PlanId = "E2",
        ElimToSemi =
        {
            S(1,3,"SF1",3),
            S(2,1,"SF1",5), S(2,4,"SF1",8),
            S(3,2,"SF1",6),
            S(4,3,"SF1",7),
            S(5,1,"SF1",4), S(5,4,"SF1",9),
            S(6,3,"SF1",2),
            S(1,1,"SF2",5), S(1,4,"SF2",8),
            S(2,2,"SF2",6),
            S(3,3,"SF2",2),
            S(4,1,"SF2",4), S(4,4,"SF2",9),
            S(5,2,"SF2",3),
            S(6,2,"SF2",7),
            S(1,2,"SF3",4),
            S(2,3,"SF3",7),
            S(3,1,"SF3",5), S(3,4,"SF3",8),
            S(4,2,"SF3",3),
            S(5,3,"SF3",2),
            S(6,1,"SF3",6), S(6,4,"SF3",9)
        },
        ElimToSemiBt = { B(5, 1, "SF2", 1), B(5, 2, "SF3", 1), B(5, 3, "SF1", 1) },
        SemiToFinalA =
        {
            S(1,1,"FA",6), S(1,2,"FA",2), S(1,3,"FA",9),
            S(2,1,"FA",5), S(2,2,"FA",3), S(2,3,"FA",8),
            S(3,1,"FA",4), S(3,2,"FA",7), S(3,3,"FA",1)
        },
        SemiToFinalB =
        {
            S(1,4,"FB",4), S(1,5,"FB",8), S(1,6,"FB",9),
            S(2,4,"FB",5), S(2,5,"FB",7), S(2,6,"FB",2),
            S(3,4,"FB",6), S(3,5,"FB",3), S(3,6,"FB",1)
        },
        SemiToFinalC =
        {
            S(1,7,"FC",6), S(1,8,"FC",2), S(1,9,"FC",9),
            S(2,7,"FC",5), S(2,8,"FC",3), S(2,9,"FC",8),
            S(3,7,"FC",4), S(3,8,"FC",7), S(3,9,"FC",1)
        }
    };

    // ═══ PLAN F ═══════════════════════════════════════════════════
    private static PlanDefinition F1() => new()
    {
        PlanId = "F1",
        ElimToSemi =
        {
            S(1,1,"SF1",5), S(1,5,"SF1",1),
            S(2,4,"SF1",8),
            S(3,4,"SF1",2),
            S(4,2,"SF1",6),
            S(5,1,"SF1",4),
            S(6,3,"SF1",7),
            S(7,2,"SF1",3),
            S(1,2,"SF2",4),
            S(2,1,"SF2",5), S(2,5,"SF2",9),
            S(3,3,"SF2",7),
            S(4,3,"SF2",3),
            S(5,2,"SF2",6),
            S(6,4,"SF2",8), S(6,5,"SF2",1),
            S(7,3,"SF2",2),
            S(1,3,"SF3",3),
            S(2,3,"SF3",7),
            S(3,1,"SF3",5), S(3,2,"SF3",6),
            S(4,4,"SF3",8),
            S(5,4,"SF3",2), S(5,5,"SF3",9),
            S(6,1,"SF3",4),
            S(7,5,"SF3",1),
            S(1,4,"SF4",2),
            S(2,2,"SF4",3),
            S(3,5,"SF4",1),
            S(4,1,"SF4",5), S(4,5,"SF4",9),
            S(5,3,"SF4",7),
            S(6,2,"SF4",6),
            S(7,1,"SF4",4), S(7,4,"SF4",8)
        },
        ElimToSemiBt = { B(6, 1, "SF4", 9) },
        SemiToFinalA =
        {
            S(1,1,"FA",5), S(1,2,"FA",8),
            S(2,1,"FA",4), S(2,2,"FA",7),
            S(3,1,"FA",6), S(3,2,"FA",2),
            S(4,1,"FA",3), S(4,2,"FA",9)
        },
        SemiToFinalBt =
        {
            B(3, 1, "FA", 1), B(3, 2, "FB", 5), B(3, 3, "FB", 6), B(3, 4, "FB", 4),
            B(5, 1, "FB", 1), B(5, 2, "FB", 9), B(5, 3, "FC", 5), B(5, 4, "FC", 6),
            B(7, 1, "FC", 8), B(7, 2, "FC", 1), B(7, 3, "FC", 9)
        },
        SemiToFinalB =
        {
            S(1,4,"FB",3), S(2,4,"FB",2), S(3,4,"FB",7), S(4,4,"FB",8)
        },
        SemiToFinalC =
        {
            S(1,6,"FC",4), S(2,6,"FC",3), S(3,6,"FC",2), S(4,6,"FC",7)
        }
    };

    private static PlanDefinition F2() => new()
    {
        PlanId = "F2",
        ElimToSemi =
        {
            S(1,3,"SF1",7),
            S(2,1,"SF1",5), S(2,5,"SF1",1),
            S(3,4,"SF1",2),
            S(4,2,"SF1",4),
            S(5,1,"SF1",6), S(5,5,"SF1",9),
            S(6,4,"SF1",8),
            S(7,3,"SF1",3),
            S(1,4,"SF2",8),
            S(2,2,"SF2",4),
            S(3,1,"SF2",6), S(3,5,"SF2",9),
            S(4,3,"SF2",3),
            S(5,2,"SF2",7),
            S(6,1,"SF2",5), S(6,5,"SF2",1),
            S(7,4,"SF2",2),
            S(1,1,"SF3",5), S(1,5,"SF3",1),
            S(2,3,"SF3",7),
            S(3,2,"SF3",4),
            S(4,4,"SF3",2),
            S(5,3,"SF3",8),
            S(6,2,"SF3",3),
            S(7,1,"SF3",6), S(7,5,"SF3",9),
            S(1,2,"SF4",4),
            S(2,4,"SF4",2),
            S(3,3,"SF4",7),
            S(4,1,"SF4",5), S(4,5,"SF4",1),
            S(5,4,"SF4",8),
            S(6,3,"SF4",3),
            S(7,2,"SF4",6)
        },
        ElimToSemiBt = { B(6, 1, "SF4", 9) },
        SemiToFinalA =
        {
            S(1,1,"FA",4), S(1,2,"FA",7),
            S(2,1,"FA",5), S(2,2,"FA",8),
            S(3,1,"FA",3), S(3,2,"FA",1),
            S(4,1,"FA",6), S(4,2,"FA",2)
        },
        SemiToFinalBt =
        {
            B(3, 1, "FA", 9),
            B(3, 2, "FB", 6), B(3, 3, "FB", 4), B(3, 4, "FB", 5),
            B(5, 1, "FB", 9), B(5, 2, "FB", 1),
            B(5, 3, "FC", 6), B(5, 4, "FC", 5),
            B(7, 1, "FC", 1), B(7, 2, "FC", 9), B(7, 3, "FC", 8)
        },
        SemiToFinalB =
        {
            S(1,4,"FB",8), S(2,4,"FB",7), S(3,4,"FB",3), S(4,4,"FB",2)
        },
        SemiToFinalC =
        {
            S(1,6,"FC",7), S(2,6,"FC",2), S(3,6,"FC",3), S(4,6,"FC",4)
        }
    };

    // ═══ PLAN G ═══════════════════════════════════════════════════
    private static PlanDefinition G1() => new()
    {
        PlanId = "G1",
        ElimToSemi =
        {
            S(1,1,"SF1",5),
            S(2,4,"SF1",2),
            S(3,3,"SF1",7),
            S(4,2,"SF1",4),
            S(5,1,"SF1",6),
            S(6,4,"SF1",9),
            S(7,3,"SF1",8),
            S(8,2,"SF1",3),
            S(1,2,"SF2",4),
            S(2,1,"SF2",5),
            S(3,4,"SF2",2),
            S(4,3,"SF2",7),
            S(5,2,"SF2",3),
            S(6,1,"SF2",6),
            S(7,4,"SF2",1),
            S(8,3,"SF2",8),
            S(1,3,"SF3",7),
            S(2,2,"SF3",4),
            S(3,1,"SF3",5),
            S(4,4,"SF3",2),
            S(5,3,"SF3",8),
            S(6,2,"SF3",3),
            S(7,1,"SF3",6),
            S(8,4,"SF3",9),
            S(1,4,"SF4",8),
            S(2,3,"SF4",7),
            S(3,2,"SF4",3),
            S(4,1,"SF4",5),
            S(5,4,"SF4",1),
            S(6,3,"SF4",2),
            S(7,2,"SF4",4),
            S(8,1,"SF4",6)
        },
        ElimToSemiBt = { B(5, 1, "SF1", 1), B(5, 2, "SF2", 9), B(5, 3, "SF3", 1), B(5, 4, "SF4", 9) },
        SemiToFinalA =
        {
            S(1,1,"FA",5), S(1,2,"FA",8),
            S(2,1,"FA",4), S(2,2,"FA",7),
            S(3,1,"FA",6), S(3,2,"FA",2),
            S(4,1,"FA",3), S(4,2,"FA",9)
        },
        SemiToFinalBt =
        {
            B(3, 1, "FA", 1),
            B(3, 2, "FB", 5), B(3, 3, "FB", 6), B(3, 4, "FB", 4),
            B(5, 1, "FB", 1), B(5, 2, "FB", 9),
            B(5, 3, "FC", 5), B(5, 4, "FC", 6),
            B(7, 1, "FC", 8), B(7, 2, "FC", 1), B(7, 3, "FC", 9)
        },
        SemiToFinalB = { S(1,4,"FB",3), S(2,4,"FB",2), S(3,4,"FB",7), S(4,4,"FB",8) },
        SemiToFinalC = { S(1,6,"FC",4), S(2,6,"FC",3), S(3,6,"FC",2), S(4,6,"FC",7) }
    };

    private static PlanDefinition G2() => new()
    {
        PlanId = "G2",
        ElimToSemi =
        {
            S(1,4,"SF1",8),
            S(2,1,"SF1",5),
            S(3,2,"SF1",6),
            S(4,3,"SF1",7),
            S(5,1,"SF1",4),
            S(6,2,"SF1",3),
            S(7,3,"SF1",2),
            S(8,4,"SF1",1),
            S(1,1,"SF2",5),
            S(2,2,"SF2",6),
            S(3,3,"SF2",7),
            S(4,4,"SF2",8),
            S(5,2,"SF2",3),
            S(6,3,"SF2",2),
            S(7,4,"SF2",9),
            S(8,1,"SF2",4),
            S(1,2,"SF3",6),
            S(2,3,"SF3",7),
            S(3,4,"SF3",8),
            S(4,1,"SF3",5),
            S(5,3,"SF3",2),
            S(6,4,"SF3",1),
            S(7,1,"SF3",4),
            S(8,2,"SF3",3),
            S(1,3,"SF4",7),
            S(2,4,"SF4",8),
            S(3,1,"SF4",5),
            S(4,2,"SF4",6),
            S(5,4,"SF4",9),
            S(6,1,"SF4",4),
            S(7,2,"SF4",3),
            S(8,3,"SF4",2)
        },
        ElimToSemiBt = { B(5, 1, "SF4", 1), B(5, 2, "SF3", 9), B(5, 3, "SF2", 1), B(5, 4, "SF1", 9) },
        SemiToFinalA =
        {
            S(1,1,"FA",3), S(1,2,"FA",1),
            S(2,1,"FA",6), S(2,2,"FA",2),
            S(3,1,"FA",5), S(3,2,"FA",8),
            S(4,1,"FA",4), S(4,2,"FA",7)
        },
        SemiToFinalBt =
        {
            B(3, 1, "FA", 9),
            B(3, 2, "FB", 6), B(3, 3, "FB", 4), B(3, 4, "FB", 5),
            B(5, 1, "FB", 9), B(5, 2, "FB", 1),
            B(5, 3, "FC", 6), B(5, 4, "FC", 5),
            B(7, 1, "FC", 1), B(7, 2, "FC", 9), B(7, 3, "FC", 8)
        },
        SemiToFinalB = { S(1,4,"FB",8), S(2,4,"FB",2), S(3,4,"FB",3), S(4,4,"FB",7) },
        SemiToFinalC = { S(1,6,"FC",7), S(2,6,"FC",4), S(3,6,"FC",3), S(4,6,"FC",2) }
    };
}
