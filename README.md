# My project

Unity-Prototyp (Unity 6000.5.3f1, URP, neues Input System). Aktueller Fokus: Player-Movement.
Einzige Gameplay-Szene: `Assets/Scenes/Test.unity`.

## Aktueller Stand

- Grundlegendes Movement über das kompositionsbasierte Lifecycle-Action-System
  (`Move`, `Jump`, `Gravity` als reine C#-Klassen, siehe `CLAUDE.md`).
- **Drehen ist smooth:** Der Character lenkt per `Quaternion.Slerp` weich in die
  Bewegungsrichtung, statt hart zu snappen. `turn_speed` ist dank
  `Time.fixedDeltaTime` frameraten-unabhängig.

## Geplante nächste Schritte

### 1. Drehen nur am Boden

In der Luft soll sich der Character nicht mehr drehen können – das ergibt kaum Sinn.

- `GroundState` in den `Move`-Konstruktor injizieren (gleiches Muster wie in `Jump`).
- Beim `new Move(...)` in `PlayerController.AllActions()` das `groundState` mitgeben.
- Den Rotations-Block in `Move.FixedTick()` mit `if (groundState.IsGrounded())` umschließen.
- **Nur** die Rotation an den Boden koppeln – die Bewegung (`linearVelocity`) bleibt
  in der Luft steuerbar (Luftkontrolle).

### 2. Momentum / Beschleunigung statt hartem Velocity-Setzen

Aktuell wird `rb.linearVelocity` jeden Physikschritt hart überschrieben – dadurch lässt
sich das gesamte Momentum sofort annullieren. Gewünschtes Verhalten:

- Aus dem Stand heraus soll die Bewegung **direkt** starten (responsiv).
- Bestehendes Momentum soll sich **nicht instant** canceln lassen (z. B. beim schnellen
  Richtungswechsel eine Kurve statt hartem Abknicken).

Umsetzung:

- Neues Tuning-Feld `[SerializeField] float acceleration;` im `PlayerController`,
  in den `Move`-Konstruktor durchreichen (wie `move_speed`).
- In `FixedTick()` die horizontale Geschwindigkeit nicht mehr setzen, sondern per
  `Vector3.MoveTowards(aktuell, ziel, acceleration * Time.fixedDeltaTime)`
  Richtung Zielgeschwindigkeit schieben. `y` (Gravity/Jump) unangetastet lassen.
- Optional später: getrennte Werte für Beschleunigen und Bremsen, falls „schnell
  anfahren, langsam ausrollen" gewünscht ist.
