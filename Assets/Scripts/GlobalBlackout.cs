using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class GlobalBlackout : MonoBehaviour
{
    [Header("Główny Materiał Serwerów")]
    public Material serverSharedMaterial;
    public string emissionColorName = "_EmissionColor";
    [ColorUsage(true, true)]
    public Color normalEmissionHDR; // Jaskrawy kolor serwerów

    [Header("Post-Processing (Kamera)")]
    public Volume globalVolume;
    public float normalExposure = 0f;       // Normalna jasność gry
    public float blackoutExposure = -4f;    // Jak ciemno ma być podczas awarii (-4 to prawie mrok)

    [Header("Parametry Awarii")]
    public float minTime = 5f;
    public float maxTime = 15f;
    public float flickerSpeed = 0.05f;
    public float blackoutDuration = 0.5f;

    private ColorAdjustments colorAdjustments;

    void Start()
    {
        // Pobieramy efekt Color Adjustments z naszego Volume
        if (globalVolume.profile.TryGet(out colorAdjustments))
        {
            SetNormalState();
            StartCoroutine(BlackoutRoutine());
        }
        else
        {
            Debug.LogError("Brak Color Adjustments w Global Volume!");
        }
    }

    private IEnumerator BlackoutRoutine()
    {
        while (true)
        {
            // Czekamy na losowy moment awarii
            yield return new WaitForSeconds(Random.Range(minTime, maxTime));

            // KROK 1: Szybkie mignięcia ostrzegawcze (zasilanie zaczyna siadać)
            for (int i = 0; i < 3; i++)
            {
                SetBlackoutState();
                yield return new WaitForSeconds(flickerSpeed);
                SetNormalState();
                yield return new WaitForSeconds(flickerSpeed);
            }

            // KROK 2: Główna awaria – TOTALNA, SAKRAMENCKA CIEMNOŚĆ
            // Suwak Blackout Duration w Inspektorze kontroluje teraz ten czas (ustaw np. 3.0)
            SetBlackoutState();
            yield return new WaitForSeconds(blackoutDuration);

            // KROK 3: Światła próbują wstać (generatory awaryjne rzężą)
            for (int i = 0; i < 4; i++)
            {
                SetNormalState();
                yield return new WaitForSeconds(flickerSpeed * 1.5f); // nieco wolniejsze błyski na rozruch
                SetBlackoutState();
                yield return new WaitForSeconds(flickerSpeed * 1.5f);
            }

            // Powrót do pełnego zasilania
            SetNormalState();
        }
    }

    private void SetNormalState()
    {
        // Włączamy neony i przywracamy normalną jasność ekranu
        serverSharedMaterial.SetColor(emissionColorName, normalEmissionHDR);
        serverSharedMaterial.EnableKeyword("_EMISSION");
        colorAdjustments.postExposure.value = normalExposure;
    }

    private void SetBlackoutState()
    {
        // Wyłączamy neony i drastycznie ściemniamy cały ekran
        serverSharedMaterial.SetColor(emissionColorName, Color.black);
        serverSharedMaterial.DisableKeyword("_EMISSION");
        colorAdjustments.postExposure.value = blackoutExposure;
    }

    // WAŻNE: Resetujemy materiał po wyłączeniu gry, żeby w edytorze nie został czarny!
    private void OnApplicationQuit()
    {
        serverSharedMaterial.SetColor(emissionColorName, normalEmissionHDR);
        serverSharedMaterial.EnableKeyword("_EMISSION");
    }
}