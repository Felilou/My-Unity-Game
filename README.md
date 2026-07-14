# My project

Unity-Prototyp (Unity 6000.5.3f1, URP, neues Input System). Aktueller Fokus: Player-Movement.
Einzige Gameplay-Szene: `Assets/Scenes/Test.unity`.

## Aktueller Stand

Movement läuft über das kompositionsbasierte Lifecycle-Action-System
(`Move`, `Jump`, `Gravity` als reine C#-Klassen, siehe `CLAUDE.md`).

- **Smooth Rotation:** Der Character lenkt per `Quaternion.Slerp` weich in die
  Bewegungsrichtung, frameraten-unabhängig via `Time.fixedDeltaTime`.
- **Drehen nur am Boden:** Rotation ist mit `groundState.IsGrounded()` geguardet –
  in der Luft dreht der Character nicht mehr.
- **Momentum / Beschleunigung:** Die horizontale Geschwindigkeit wird per
  `Vector3.MoveTowards` mit `accellaration` Richtung Ziel geschoben statt hart gesetzt.
  Aus dem Stand startet die Bewegung direkt, bestehendes Momentum wird nicht mehr
  instant gecancelt (Kurve statt hartem Richtungswechsel).

## Geplante nächste Schritte

### Tuning-Werte zur Laufzeit einstellbar machen

Problem: Die `[SerializeField]`-Werte im `PlayerController` werden in `Awake` **einmal**
per Konstruktor in die Actions kopiert (`readonly`-Felder). Inspector-Änderungen kommen
danach nicht mehr an → schlecht zum Ausprobieren der besten Werte.

Angedachte Richtung (noch offen):

- Tuning pro Action in ein **eigenes Config-Objekt** auslagern, das die Action per
  **Referenz** hält und live liest (statt kopierter floats).
- Möglicher Weg A: `[SerializeReference]`-Liste polymorpher Config-Objekte, jede Config
  erzeugt ihre Action selbst (Config = Fabrik) → Actions registrieren sich quasi selbst.
- Möglicher Weg B: pro Action ein konkretes `[SerializeField <Config>]`-Feld am Handler
  (kein `[SerializeReference]`, dafür manuell gelistet, aber typsicher).
- Für persistente Werte über den Play Mode hinaus: Config als `ScriptableObject`.

Entscheidung zwischen Auto-Registrierung (Weg A) und maximaler Typsicherheit (Weg B)
steht noch aus.
