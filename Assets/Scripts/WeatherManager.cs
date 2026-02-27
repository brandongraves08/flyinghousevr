using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    [System.Serializable]
    public class WeatherState
    {
        public string name = "Clear";
        public float turbulence = 0f;
        public float visibility = 1f;
        public float rainIntensity = 0f;
        public float fogDensity = 0f;
        public float windSpeed = 10f;
        public Color skyColor = Color.cyan;
    }

    [Header("Weather States")]
    public WeatherState clear = new WeatherState { name = "Clear", skyColor = Color.cyan };
    public WeatherState cloudy = new WeatherState { name = "Cloudy", turbulence = 0.2f, visibility = 0.8f, fogDensity = 0.1f, skyColor = Color.gray };
    public WeatherState rain = new WeatherState { name = "Rain", turbulence = 0.4f, visibility = 0.6f, rainIntensity = 0.5f, fogDensity = 0.2f, windSpeed = 20f, skyColor = Color.gray };
    public WeatherState storm = new WeatherState { name = "Storm", turbulence = 0.7f, visibility = 0.4f, rainIntensity = 1f, fogDensity = 0.4f, windSpeed = 40f, skyColor = Color.black };

    [Header("Current State")]
    public string currentWeather = "Clear";
    public float transitionSpeed = 2f;

    [Header("Effects")]
    public ParticleSystem rainEffect;
    public ParticleSystem windEffect;
    public Light ambientLight;
    public AudioSource rainAudio;
    public AudioSource windAudio;

    private FlightController flightController;
    private float currentTurbulence;
    private float transitionTime;

    void Start()
    {
        flightController = FindObjectOfType<FlightController>();
        SetWeather(currentWeather, true);
    }

    public void SetWeather(string weatherName, bool instant = false)
    {
        WeatherState newState = GetState(weatherName);
        if (newState == null) return;

        currentWeather = weatherName;
        transitionTime = 0f;

        if (instant)
        {
            ApplyState(newState, 1f);
        }
    }

    WeatherState GetState(string name)
    {
        switch (name.ToLower())
        {
            case "clear": return clear;
            case "cloudy": return cloudy;
            case "rain": return rain;
            case "storm": return storm;
            default: return clear;
        }
    }

    void Update()
    {
        WeatherState targetState = GetState(currentWeather);
        if (targetState == null) return;

        transitionTime += Time.deltaTime / transitionSpeed;
        float t = Mathf.Clamp01(transitionTime);

        ApplyState(targetState, t);
    }

    void ApplyState(WeatherState state, float t)
    {
        // Turbulence affects steering responsiveness
        currentTurbulence = Mathf.Lerp(currentTurbulence, state.turbulence, t);
        
        if (flightController != null)
        {
            flightController.turnSpeed = 30f * (1f - currentTurbulence * 0.5f);
        }

        // Rain effect
        if (rainEffect != null)
        {
            var emission = rainEffect.emission;
            emission.rateOverTime = Mathf.Lerp(0, 1000, state.rainIntensity * t);
        }

        // Wind particles
        if (windEffect != null)
        {
            var emission = windEffect.emission;
            emission.rateOverTime = state.windSpeed * t * 2f;
        }

        // Lighting
        if (ambientLight != null)
        {
            ambientLight.intensity = Mathf.Lerp(1f, 0.3f, state.fogDensity * t);
        }

        // Audio
        if (rainAudio != null)
        {
            rainAudio.volume = Mathf.Lerp(0, state.rainIntensity * 0.8f, t);
        }
        
        if (windAudio != null)
        {
            windAudio.volume = Mathf.Lerp(0.1f, state.windSpeed / 50f, t);
        }
    }

    public void RandomizeWeather()
    {
        string[] options = { "Clear", "Cloudy", "Rain", "Storm" };
        string newWeather = options[Random.Range(0, options.Length)];
        SetWeather(newWeather);
    }
}
