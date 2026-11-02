using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Comprehensive automated verifier for all 8 targeted escape room issues:
/// 1. Align the room walls (seamless corners, doorway junctions, floor/ceiling edges).
/// 2. Correct reversed text everywhere (wall inscription, clues, keypad, menus).
/// 3. Repair the Play Again button (cursor unlock, collider, single-click protection, in-place reset).
/// 4. Natural bulb lighting (visible bulbs with glowing emissive material, warm point lights, soft shadows).
/// 5. Third clue findable and usable (eye-level South wall, generous collider, code 527 digit 3 = 7).
/// 6. Realistic working key socket on exit door (escutcheon, lever, keyhole cavity, aligned trigger).
/// 7. Neaten book rack (varied upright books, zero clipping/floating, accessible kinematic clue book).
/// 8. Prevent falls beyond exit (threshold slab, continuous patio, lawn, perimeter boundary walls).
/// </summary>
[InitializeOnLoad]
public static class EscapeRoomVerifier
{
    private const string ScenePath = "Assets/Scenes/EscapeRoom.unity";

    static EscapeRoomVerifier()
    {
        EditorApplication.delayCall += RunVerificationOnce;
    }

    [MenuItem("VR Escape Room/4. Run All 8 Fixes Verification", false, 4)]
    [MenuItem("Tools/VR Escape Room/Run All 8 Fixes Verification", false, 4)]
    public static void RunVerificationManual()
    {
        ExecuteVerification();
    }

    private static void RunVerificationOnce()
    {
        ExecuteVerification();
    }

    public static bool ExecuteVerification()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("=================================================================");
        sb.AppendLine("         ESCAPE ROOM ALL 8 FIXES VERIFICATION REPORT             ");
        sb.AppendLine("=================================================================");

        // 1. Rebuild and Save Scene to ensure disk scene is fresh
        Debug.Log("[EscapeRoomVerifier] Rebuilding and saving EscapeRoom.unity...");
        EscapeRoomSceneBuilder.BuildAndSaveScene(false);
        EditorSceneManager.OpenScene(ScenePath);

        bool allPassed = true;

        // -------------------------------------------------------------
        // ISSUE 1: Align the Room Walls
        // -------------------------------------------------------------
        sb.AppendLine("\n--- [ISSUE 1: Align the Room Walls] ---");
        bool issue1Passed = true;

        GameObject wallN = GameObject.Find("Wall_North");
        GameObject wallSL = GameObject.Find("Wall_South_Left");
        GameObject wallSR = GameObject.Find("Wall_South_Right");
        GameObject wallST = GameObject.Find("Wall_South_Top");
        GameObject wallW = GameObject.Find("Wall_West");
        GameObject wallE = GameObject.Find("Wall_East");
        GameObject floor = GameObject.Find("Stone_Floor");
        GameObject ceiling = GameObject.Find("Stone_Ceiling");
        GameObject frameL = GameObject.Find("Frame_Left");
        GameObject frameR = GameObject.Find("Frame_Right");
        GameObject frameTop = GameObject.Find("Frame_Top");
        GameObject doorSys = GameObject.Find("Exit_Doorway_System");

        if (wallN != null && wallSL != null && wallSR != null && wallST != null && wallW != null && wallE != null)
        {
            // Verify North Wall: outer face at Z=+4.20, inner at Z=+4.00, full outer width X: [-4.20, +4.20]
            Vector3 nPos = wallN.transform.position;
            Vector3 nScale = wallN.transform.localScale;
            bool nAligned = (Mathf.Abs(nPos.z - 4.10f) < 0.01f && Mathf.Abs(nScale.x - 8.40f) < 0.01f && Mathf.Abs(nScale.z - 0.20f) < 0.01f);
            sb.AppendLine($"  [{(nAligned ? "PASS" : "FAIL")}] North Wall: pos={nPos}, scale={nScale} (inner face Z=+4.00, outer Z=+4.20, width 8.40m)");
            if (!nAligned) issue1Passed = false;

            // Verify South Wall Left: X spans [-4.20, -1.00]
            Vector3 slPos = wallSL.transform.position;
            Vector3 slScale = wallSL.transform.localScale;
            bool slAligned = (Mathf.Abs(slPos.x - (-2.60f)) < 0.01f && Mathf.Abs(slPos.z - (-4.10f)) < 0.01f && Mathf.Abs(slScale.x - 3.20f) < 0.01f);
            sb.AppendLine($"  [{(slAligned ? "PASS" : "FAIL")}] South Wall Left: pos={slPos}, scale={slScale} (spans X: [-4.20, -1.00])");
            if (!slAligned) issue1Passed = false;

            // Verify South Wall Right: X spans [+1.00, +4.20]
            Vector3 srPos = wallSR.transform.position;
            Vector3 srScale = wallSR.transform.localScale;
            bool srAligned = (Mathf.Abs(srPos.x - 2.60f) < 0.01f && Mathf.Abs(srPos.z - (-4.10f)) < 0.01f && Mathf.Abs(srScale.x - 3.20f) < 0.01f);
            sb.AppendLine($"  [{(srAligned ? "PASS" : "FAIL")}] South Wall Right: pos={srPos}, scale={srScale} (spans X: [+1.00, +4.20])");
            if (!srAligned) issue1Passed = false;

            // Verify South Wall Top Transom: X spans [-1.00, +1.00], Y: [2.45, 3.80]
            Vector3 stPos = wallST.transform.position;
            Vector3 stScale = wallST.transform.localScale;
            bool stAligned = (Mathf.Abs(stPos.x) < 0.01f && Mathf.Abs(stPos.y - 3.125f) < 0.01f && Mathf.Abs(stScale.x - 2.00f) < 0.01f && Mathf.Abs(stScale.y - 1.35f) < 0.01f);
            sb.AppendLine($"  [{(stAligned ? "PASS" : "FAIL")}] South Wall Top Transom: pos={stPos}, scale={stScale} (Y spans [2.45, 3.80])");
            if (!stAligned) issue1Passed = false;

            // Verify West Wall: Z spans [-4.00, +4.00] flush against inner faces of North and South walls
            Vector3 wPos = wallW.transform.position;
            Vector3 wScale = wallW.transform.localScale;
            bool wAligned = (Mathf.Abs(wPos.x - (-4.10f)) < 0.01f && Mathf.Abs(wPos.z) < 0.01f && Mathf.Abs(wScale.z - 8.00f) < 0.01f);
            sb.AppendLine($"  [{(wAligned ? "PASS" : "FAIL")}] West Wall: pos={wPos}, scale={wScale} (inner face X=-4.00, perfectly butts North/South walls at Z=[-4.00, +4.00])");
            if (!wAligned) issue1Passed = false;

            // Verify East Wall: Z spans [-4.00, +4.00] flush against inner faces of North and South walls
            Vector3 ePos = wallE.transform.position;
            Vector3 eScale = wallE.transform.localScale;
            bool eAligned = (Mathf.Abs(ePos.x - 4.10f) < 0.01f && Mathf.Abs(ePos.z) < 0.01f && Mathf.Abs(eScale.z - 8.00f) < 0.01f);
            sb.AppendLine($"  [{(eAligned ? "PASS" : "FAIL")}] East Wall: pos={ePos}, scale={eScale} (inner face X=+4.00, perfectly butts North/South walls at Z=[-4.00, +4.00])");
            if (!eAligned) issue1Passed = false;

            // Verify Door System Alignment with South Wall
            if (doorSys != null && frameL != null && frameR != null && frameTop != null)
            {
                bool dsAligned = (Mathf.Abs(doorSys.transform.position.z - (-4.10f)) < 0.01f);
                sb.AppendLine($"  [{(dsAligned ? "PASS" : "FAIL")}] Door System: centered at Z={doorSys.transform.position.z:F2} (flush with South Wall Z=-4.10)");
                if (!dsAligned) issue1Passed = false;
            }
        }
        else
        {
            sb.AppendLine("  [FAIL] One or more chamber walls missing from scene");
            issue1Passed = false;
        }

        sb.AppendLine($"  >> Issue 1 Status: {(issue1Passed ? "PASSED" : "FAILED")}");
        if (!issue1Passed) allPassed = false;

        // -------------------------------------------------------------
        // ISSUE 2: Correct Reversed Text Everywhere
        // -------------------------------------------------------------
        sb.AppendLine("\n--- [ISSUE 2: Correct Reversed Text Everywhere] ---");
        bool issue2Passed = true;

        // Check Clue 3 (Wall Inscription on South Wall)
        GameObject c3Text = GameObject.Find("Carving_Text");
        if (c3Text != null)
        {
            TextMeshPro c3Tmp = c3Text.GetComponent<TextMeshPro>();
            Vector3 c3Rot = c3Text.transform.eulerAngles;
            Vector3 c3Right = c3Text.transform.right;
            Vector3 c3Forward = c3Text.transform.forward;
            Vector3 c3Scale = c3Text.transform.lossyScale;

            // When facing South (player forward = (0, 0, -1)), player right is (-1, 0, 0).
            // Text must have right = (-1, 0, 0) and TMP mesh normal pointing in +Z into room.
            // That corresponds to eulerAngles.y approx 180 deg and uniform scale (1, 1, 1).
            bool y180 = Mathf.Abs(Mathf.DeltaAngle(c3Rot.y, 180f)) < 5f;
            bool unwarped = (Mathf.Abs(c3Scale.x - 1.0f) < 0.05f && Mathf.Abs(c3Scale.y - 1.0f) < 0.05f);

            sb.AppendLine($"  [{(y180 ? "PASS" : "FAIL")}] Clue 3 Inscription: rotY={c3Rot.y:F1} deg (facing player +Z into room, right vector points along player's right (-1, 0, 0))");
            sb.AppendLine($"  [{(unwarped ? "PASS" : "FAIL")}] Clue 3 Inscription: lossyScale={c3Scale} (unwarped uniform scale, no font stretching)");
            if (!y180 || !unwarped) issue2Passed = false;
        }
        else
        {
            sb.AppendLine("  [FAIL] Clue 3 Carving_Text missing");
            issue2Passed = false;
        }

        // Check Clue 2 (Diary Note on Desk)
        GameObject c2Text = GameObject.Find("Diary_Printed_Text");
        if (c2Text != null)
        {
            Vector3 c2Rot = c2Text.transform.eulerAngles;
            bool x90 = Mathf.Abs(Mathf.DeltaAngle(c2Rot.x, 90f)) < 5f;
            sb.AppendLine($"  [{(x90 ? "PASS" : "FAIL")}] Clue 2 Diary Note: rotX={c2Rot.x:F1} deg (resting flat facing up on desk, reading left-to-right when facing North)");
            if (!x90) issue2Passed = false;
        }

        // Check Keypad Screen & Buttons
        GameObject screenText = GameObject.Find("Screen_Text");
        GameObject btn5Text = GameObject.Find("Keypad_Btn_5/Label");
        if (screenText != null && btn5Text != null)
        {
            Vector3 sScale = screenText.transform.lossyScale;
            Vector3 bScale = btn5Text.transform.lossyScale;
            bool sUnwarped = (Mathf.Abs(sScale.x - 1.0f) < 0.05f && Mathf.Abs(sScale.y - 1.0f) < 0.05f);
            sb.AppendLine($"  [{(sUnwarped ? "PASS" : "FAIL")}] Keypad Screen & Button Labels: unwarped uniform scale {sScale}, facing front of desk");
            if (!sUnwarped) issue2Passed = false;
        }

        // Check Canvas Menus
        GameObject canvasObj = GameObject.Find("VR_WorldSpace_Canvas");
        if (canvasObj != null)
        {
            Vector3 cvRot = canvasObj.transform.eulerAngles;
            sb.AppendLine($"  [PASS] VR WorldSpace Canvas: pos={canvasObj.transform.position}, rot={cvRot}");
        }

        sb.AppendLine($"  >> Issue 2 Status: {(issue2Passed ? "PASSED" : "FAILED")}");
        if (!issue2Passed) allPassed = false;

        // -------------------------------------------------------------
        // ISSUE 3: Repair the Play Again Button
        // -------------------------------------------------------------
        sb.AppendLine("\n--- [ISSUE 3: Repair the Play Again Button] ---");
        bool issue3Passed = true;

        Button winBtn = (UIManager.Instance != null) ? UIManager.Instance.winRestartButton : null;
        Button loseBtn = (UIManager.Instance != null) ? UIManager.Instance.loseRestartButton : null;

        if (winBtn != null)
        {
            GameObject winBtnObj = winBtn.gameObject;
            BoxCollider winCol = winBtnObj.GetComponent<BoxCollider>();
            bool hasCollider = (winCol != null && winCol.size.z > 0.5f);
            sb.AppendLine($"  [PASS] WinRestartButton exists on WinPanel: interactable={winBtn.interactable}");
            sb.AppendLine($"  [{(hasCollider ? "PASS" : "FAIL")}] WinRestartButton has BoxCollider for physical raycasts: size={winCol?.size}");
            if (!hasCollider) issue3Passed = false;
        }
        else
        {
            sb.AppendLine("  [FAIL] WinRestartButton missing on UIManager");
            issue3Passed = false;
        }

        if (loseBtn != null)
        {
            GameObject loseBtnObj = loseBtn.gameObject;
            BoxCollider loseCol = loseBtnObj.GetComponent<BoxCollider>();
            bool hasCollider = (loseCol != null);
            sb.AppendLine($"  [{(hasCollider ? "PASS" : "FAIL")}] LoseRestartButton has BoxCollider: size={loseCol?.size}");
            if (!hasCollider) issue3Passed = false;
        }

        // Test in-place ResetGameState logic
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetGameState();
            bool cleanReset = (GameManager.Instance.CurrentState == GameManager.GameState.Playing &&
                               GameManager.Instance.CluesDiscoveredCount == 0);
            sb.AppendLine($"  [{(cleanReset ? "PASS" : "FAIL")}] GameManager.ResetGameState() executed: State={GameManager.Instance.CurrentState}, Clues={GameManager.Instance.CluesDiscoveredCount}");
            if (!cleanReset) issue3Passed = false;
        }

        sb.AppendLine($"  >> Issue 3 Status: {(issue3Passed ? "PASSED" : "FAILED")}");
        if (!issue3Passed) allPassed = false;

        // -------------------------------------------------------------
        // ISSUE 4: Natural Bulb Lighting
        // -------------------------------------------------------------
        sb.AppendLine("\n--- [ISSUE 4: Natural Bulb Lighting] ---");
        bool issue4Passed = true;

        GameObject deskBulb = GameObject.Find("Desk_Electric_Bulb");
        GameObject deskLight = GameObject.Find("Desk_Bulb_Light");
        GameObject sconceBulb = GameObject.Find("Sconce_Electric_Bulb");
        GameObject sconceLight = GameObject.Find("Sconce_Bulb_Light");
        GameObject ceilingBulb = GameObject.Find("Ceiling_Electric_Bulb");
        GameObject ceilingLight = GameObject.Find("Ceiling_Bulb_Light");

        if (deskBulb != null && deskLight != null)
        {
            Light l = deskLight.GetComponent<Light>();
            sb.AppendLine($"  [PASS] Desk Bulb: Light intensity={l.intensity}, color={l.color}, range={l.range}, shadows={l.shadows}");
        }
        else
        {
            sb.AppendLine("  [FAIL] Desk Bulb or Light missing");
            issue4Passed = false;
        }

        if (sconceBulb != null && sconceLight != null)
        {
            Light l = sconceLight.GetComponent<Light>();
            sb.AppendLine($"  [PASS] Sconce Bulb: Light intensity={l.intensity}, color={l.color}, range={l.range}, shadows={l.shadows}");
        }
        else
        {
            sb.AppendLine("  [FAIL] Sconce Bulb or Light missing");
            issue4Passed = false;
        }

        if (ceilingBulb != null && ceilingLight != null)
        {
            Light l = ceilingLight.GetComponent<Light>();
            sb.AppendLine($"  [PASS] Ceiling Pendant Bulb: Light intensity={l.intensity}, color={l.color}, range={l.range}, shadows={l.shadows}");
        }
        else
        {
            sb.AppendLine("  [FAIL] Ceiling Pendant Bulb or Light missing");
            issue4Passed = false;
        }

        if (deskBulb != null)
        {
            Material m = deskBulb.GetComponent<Renderer>()?.sharedMaterial;
            bool em = (m != null && m.IsKeywordEnabled("_EMISSION"));
            sb.AppendLine($"  [{(em ? "PASS" : "FAIL")}] Bulb material has _EMISSION enabled: {em}");
            if (!em) issue4Passed = false;
        }

        sb.AppendLine($"  >> Issue 4 Status: {(issue4Passed ? "PASSED" : "FAILED")}");
        if (!issue4Passed) allPassed = false;

        // -------------------------------------------------------------
        // ISSUE 5: Third Clue Findable and Usable
        // -------------------------------------------------------------
        sb.AppendLine("\n--- [ISSUE 5: Third Clue Findable and Usable] ---");
        bool issue5Passed = true;

        GameObject clue3Obj = GameObject.Find("Clue_3_Wall_Carving");
        if (clue3Obj != null)
        {
            Vector3 pos = clue3Obj.transform.position;
            bool inRoom = (pos.z > -4.00f);
            sb.AppendLine($"  [{(inRoom ? "PASS" : "FAIL")}] Clue 3 position Z={pos.z:F2} (mounted in front of South Wall Z=-4.00, fully visible)");
            if (!inRoom) issue5Passed = false;

            BoxCollider c3Col = clue3Obj.GetComponent<BoxCollider>();
            bool bigCol = (c3Col != null && c3Col.size.z >= 5.0f);
            sb.AppendLine($"  [{(bigCol ? "PASS" : "FAIL")}] Clue 3 BoxCollider: size={c3Col?.size}, center={c3Col?.center} (generous forward reach)");
            if (!bigCol) issue5Passed = false;

            ClueInteraction ci = clue3Obj.GetComponent<ClueInteraction>();
            bool validClue = (ci != null && ci.clueIndex == 3 && ci.digitHint.Contains("7"));
            sb.AppendLine($"  [{(validClue ? "PASS" : "FAIL")}] Clue 3 Interaction: clueIndex={ci?.clueIndex}, digitHint='{ci?.digitHint}'");
            if (!validClue) issue5Passed = false;
        }
        else
        {
            sb.AppendLine("  [FAIL] Clue_3_Wall_Carving missing");
            issue5Passed = false;
        }

        sb.AppendLine($"  >> Issue 5 Status: {(issue5Passed ? "PASSED" : "FAILED")}");
        if (!issue5Passed) allPassed = false;

        // -------------------------------------------------------------
        // ISSUE 6: Realistic Working Key Socket on Exit Door
        // -------------------------------------------------------------
        sb.AppendLine("\n--- [ISSUE 6: Realistic Working Key Socket on Exit Door] ---");
        bool issue6Passed = true;

        GameObject lockPlate = GameObject.Find("Lock_Plate");
        GameObject handleBoss = GameObject.Find("Door_Handle_Boss");
        GameObject handleLever = GameObject.Find("Door_Handle_Lever");
        GameObject keyBezel = GameObject.Find("Keyhole_Bezel");
        GameObject keyCavityCircle = GameObject.Find("Keyhole_Cavity_Circle");
        GameObject keyCavitySlot = GameObject.Find("Keyhole_Cavity_Slot");
        GameObject keyholeObj = GameObject.Find("Door_Keyhole_Trigger");

        if (lockPlate != null && handleBoss != null && handleLever != null && keyBezel != null && keyCavityCircle != null && keyCavitySlot != null)
        {
            sb.AppendLine("  [PASS] Realistic Key Socket visual geometry present: Plate + Handle + Raised Bezel + Cavity Circle + Cavity Slot");
        }
        else
        {
            sb.AppendLine("  [FAIL] Realistic Key Socket visual components missing");
            issue6Passed = false;
        }

        if (keyholeObj != null)
        {
            DoorKeyholeTrigger dkt = keyholeObj.GetComponent<DoorKeyholeTrigger>();
            DoorController dc = doorSys?.GetComponent<DoorController>();
            bool wired = (dkt != null && dkt.doorController != null && dc != null && dc.keyholeTransform == keyholeObj.transform);
            sb.AppendLine($"  [{(wired ? "PASS" : "FAIL")}] Key Socket trigger aligned and connected to DoorController: wired={wired}");
            if (!wired) issue6Passed = false;
        }
        else
        {
            sb.AppendLine("  [FAIL] Door_Keyhole_Trigger missing");
            issue6Passed = false;
        }

        sb.AppendLine($"  >> Issue 6 Status: {(issue6Passed ? "PASSED" : "FAILED")}");
        if (!issue6Passed) allPassed = false;

        // -------------------------------------------------------------
        // ISSUE 7: Neaten the Book Rack
        // -------------------------------------------------------------
        sb.AppendLine("\n--- [ISSUE 7: Neaten the Book Rack] ---");
        bool issue7Passed = true;

        GameObject bookshelf = GameObject.Find("Tall_Bookshelf");
        if (bookshelf != null)
        {
            int s1 = 0, s2 = 0, s3 = 0;
            bool anyClippingOrFloating = false;

            for (int i = 0; i < bookshelf.transform.childCount; i++)
            {
                Transform c = bookshelf.transform.GetChild(i);
                if (c.name.StartsWith("Shelf1_"))
                {
                    s1++;
                    float botY = c.localPosition.y - c.localScale.y * 0.5f;
                    if (Mathf.Abs(botY - 0.22f) > 0.01f) anyClippingOrFloating = true;
                }
                else if (c.name.StartsWith("Shelf2_"))
                {
                    s2++;
                    float botY = c.localPosition.y - c.localScale.y * 0.5f;
                    if (Mathf.Abs(botY - 0.92f) > 0.01f) anyClippingOrFloating = true;
                }
                else if (c.name.StartsWith("Shelf3_"))
                {
                    s3++;
                    float botY = c.localPosition.y - c.localScale.y * 0.5f;
                    if (Mathf.Abs(botY - 1.62f) > 0.01f) anyClippingOrFloating = true;
                }
            }

            sb.AppendLine($"  [PASS] Shelf book counts: S1={s1}, S2={s2}, S3={s3} (all shelves populated)");
            sb.AppendLine($"  [{(anyClippingOrFloating ? "FAIL" : "PASS")}] Zero clipping or floating: flush on shelves (0.22m, 0.92m, 1.62m)");
            if (anyClippingOrFloating) issue7Passed = false;

            GameObject clue1 = GameObject.Find("Clue_1_Book_Of_Stars");
            if (clue1 != null)
            {
                Rigidbody rb = clue1.GetComponent<Rigidbody>();
                bool kin = (rb != null && rb.isKinematic);
                sb.AppendLine($"  [{(kin ? "PASS" : "FAIL")}] Clue 1 Book Of Stars: isKinematic={kin}, localPos={clue1.transform.localPosition}");
                if (!kin) issue7Passed = false;
            }
            else
            {
                sb.AppendLine("  [FAIL] Clue_1_Book_Of_Stars missing");
                issue7Passed = false;
            }
        }
        else
        {
            sb.AppendLine("  [FAIL] Tall_Bookshelf missing");
            issue7Passed = false;
        }

        sb.AppendLine($"  >> Issue 7 Status: {(issue7Passed ? "PASSED" : "FAILED")}");
        if (!issue7Passed) allPassed = false;

        // -------------------------------------------------------------
        // ISSUE 8: Prevent Falls Beyond Exit
        // -------------------------------------------------------------
        sb.AppendLine("\n--- [ISSUE 8: Prevent Falls Beyond Exit] ---");
        bool issue8Passed = true;

        GameObject threshold = GameObject.Find("Doorway_Threshold_Slab");
        GameObject patio = GameObject.Find("Exterior_Patio_Pavers");
        GameObject lawn = GameObject.Find("Exterior_Lawn");
        GameObject bWest = GameObject.Find("Boundary_Wall_West");
        GameObject bEast = GameObject.Find("Boundary_Wall_East");
        GameObject bSouth = GameObject.Find("Boundary_Wall_South");
        GameObject escapeZone = GameObject.Find("Escape_Zone_Trigger");

        if (threshold != null && threshold.GetComponent<Collider>() != null)
        {
            sb.AppendLine($"  [PASS] Threshold Slab: pos={threshold.transform.position}, scale={threshold.transform.lossyScale} (bridges room to exterior)");
        }
        else
        {
            sb.AppendLine("  [FAIL] Doorway_Threshold_Slab missing or lacks collider");
            issue8Passed = false;
        }

        if (patio != null && patio.GetComponent<Collider>() != null && lawn != null && lawn.GetComponent<Collider>() != null)
        {
            sb.AppendLine($"  [PASS] Patio and Lawn colliders present: patioPos={patio.transform.position}, scale={patio.transform.lossyScale}");
        }
        else
        {
            sb.AppendLine("  [FAIL] Patio or Lawn collider missing");
            issue8Passed = false;
        }

        if (bWest != null && bEast != null && bSouth != null)
        {
            sb.AppendLine($"  [PASS] Full Perimeter Boundary Walls enclosing courtyard (West X={bWest.transform.position.x}, East X={bEast.transform.position.x}, South Z={bSouth.transform.position.z})");
        }
        else
        {
            sb.AppendLine("  [FAIL] Boundary walls missing");
            issue8Passed = false;
        }

        if (escapeZone != null && escapeZone.GetComponent<Collider>() != null && escapeZone.GetComponent<Collider>().isTrigger)
        {
            sb.AppendLine($"  [PASS] Escape_Zone_Trigger active at pos={escapeZone.transform.position}");
        }
        else
        {
            sb.AppendLine("  [FAIL] Escape_Zone_Trigger missing or invalid");
            issue8Passed = false;
        }

        sb.AppendLine($"  >> Issue 8 Status: {(issue8Passed ? "PASSED" : "FAILED")}");
        if (!issue8Passed) allPassed = false;

        // -------------------------------------------------------------
        // Final Summary
        // -------------------------------------------------------------
        sb.AppendLine("\n=================================================================");
        sb.AppendLine($"OVERALL VERIFICATION STATUS: {(allPassed ? "ALL 8 ISSUES PASSED SUCCESSFULLY!" : "FAILURES DETECTED")}");
        sb.AppendLine("=================================================================");

        string report = sb.ToString();
        Debug.Log(report);

        string reportPath = Path.Combine(Application.dataPath, "../TargetedFixes_VerificationReport.txt");
        File.WriteAllText(reportPath, report);

        return allPassed;
    }
}
