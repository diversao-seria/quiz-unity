using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PowerUps
{
    public GameMechanicsConstant.PowerUpNames name;
    public Sprite icon;
    public Color color;

}
public class PowerUpController : MonoBehaviour
{
    public static int rightAnswerCount = 0;
    public bool leafImmunity = false, timeFreeze = false;
    public int fireComboNumber = 3, airRemovedAnswers = 2;
    public GameObject powerUpButtonPrefab, powerUpAnimation;
    public Transform powerUpWrapper;
    public List<PowerUps> powerUps = new List<PowerUps>();
    [HideInInspector]
    public List<GameObject> powerUpGameObjects = new List<GameObject>();
    public GameObject Background;
    public int a1, a2;
    public int[] randomIndex;
    public AudioClip[] audioClips;
    public Clock FreezeClock { get; set; }
    public Color backgroundColor;
    public RuntimeAnimatorController vento, gelo, folha; 

    private AudioSource audioSource;
    private JsonController jsonController;
    private bool airPowerUp = false;
    private int[] indexArray;
    private Image bg;
    private Color grey = new Color(0.35294117647f, 0.35294117647f, 0.35294117647f);
    private GameObject[] answers;
    private Animator animator;
    private bool isFuryMode = false;

    private enum Clip : int
    {
        water,
        air,
        earth
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        animator = powerUpAnimation.GetComponent<Animator>();
        bg = Background.GetComponent<Image>();
        bg.color = backgroundColor;
        powerUpAnimation.SetActive(false);
        FreezeClock = new Clock(0.0f);
        RestoreAllPowerUps();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AnswerCount(bool isCorrect)
    {
        if (isCorrect)
        {
            rightAnswerCount++;
            if (rightAnswerCount == fireComboNumber)
            {
                EnableFuryMode();
            }
        }
        else
        {
            if (leafImmunity)
            {
                LeafPowerExpired();
                bg.color = backgroundColor;
            }
            else
            {
                DisableFuryMode();
            }
        }
    }

    private void EnableFuryMode()
    {
        Debug.Log("Fire");
        bg.color = Color.red;
        isFuryMode = true;
        RestoreAllPowerUps();
    }

    private void DisableFuryMode()
    {
        rightAnswerCount = 0;
        isFuryMode = false;
        bg.color = backgroundColor;
    }

    public void RestoreAllPowerUps()
    {
        RemoveAllPowerUps();

        for (int i = 0; i < powerUps.Count; i++)
        {

            // PowerUps pw = new PowerUps(GameMechanicsConstant.PowerUpNames[i],,
            RestorePowerUp(powerUps[i]);
        }
    }

    public void RemoveAllPowerUps()
    {
        while (powerUpGameObjects.Count > 0)
        {
            RemovePowerUp(powerUpGameObjects[0]);
        }
    }


    public void RestorePowerUp(PowerUps pw)
    {
        GameObject powerUpGameObject = Instantiate(powerUpButtonPrefab);
        powerUpGameObject.transform.SetParent(powerUpWrapper);
        powerUpGameObjects.Add(powerUpGameObject);
        Button button = powerUpGameObject.GetComponent<Button>();

        PowerUpButton powerUpButton = powerUpGameObject.GetComponent<PowerUpButton>();
        button.onClick.AddListener(delegate { powerUpButton.HandleClick(); });
        powerUpButton.powerUp = pw.name;
        // powerUpGameObject.GetComponentInChildren<Image>().color = pw.color;
        powerUpGameObject.GetComponentInChildren<Image>().sprite = pw.icon;

    }

    public void RemovePowerUp(GameObject pw)
    {
        int powerUpIndex = -1;
        for (int i = 0; i < powerUpGameObjects.Count; i++)
        {
            if (pw.GetComponent<PowerUpButton>().powerUp == powerUpGameObjects[i].GetComponent<PowerUpButton>().powerUp)
            {
                powerUpIndex = i;
            }
        }
        if (powerUpIndex != -1)
        {
            Destroy(powerUpGameObjects[powerUpIndex]);
            powerUpGameObjects.RemoveAt(powerUpIndex);
        }
    }
    public void WaterPowerUp()
    {
        timeFreeze = true;
        StartCoroutine(PowerUpGeloAnim());
        jsonController.UsedPowerUp(GameMechanicsConstant.PowerUpNames.Water);
    }

    public void WaterPowerExpired() 
    {
        timeFreeze = false;

        if (isFuryMode)
        {
            bg.color = Color.red;
        }
        else
        {
            bg.color = backgroundColor;
        }

        FreezeClock.Reset();
    }

    public void AirPowerUp()
    {
        answers = GameObject.FindGameObjectsWithTag("Answer");
        List<int> indexes = new List<int>();

        airPowerUp = true;
        StartCoroutine(PowerUpVentoAnim());

        for (int i = 0; i < answers.Length; i++)
        {
            AnswerButton answerButton = answers[i].GetComponent<AnswerButton>();
            if (!answerButton.alternative.IsCorrect)
            {
                indexes.Add(i);
            }
        }

        indexArray = indexes.ToArray();
        randomIndex = ReturnRandomIndexes(indexArray, airRemovedAnswers);

        for (int i = 0; i < airRemovedAnswers; i++)
        {

            answers[indexArray[randomIndex[i]]].GetComponent<Image>().enabled = false;
            answers[indexArray[randomIndex[i]]].GetComponent<Button>().enabled = false;
            answers[indexArray[randomIndex[i]]].transform.GetChild(0).gameObject.SetActive(false);
        }

        jsonController.UsedPowerUp(GameMechanicsConstant.PowerUpNames.Air);
    }

    public void AirPowerExpired()
    {
        airPowerUp = false;

        if (answers != null)
        {
            for (int i = 0; i < answers.Length; i++)
            {
                answers[i].GetComponent<Image>().enabled = true;
                answers[i].GetComponent<Button>().enabled = true;
                answers[i].transform.GetChild(0).gameObject.SetActive(true);
            }
        }

        if (isFuryMode)
        {
            bg.color = Color.red;
        }
        else
        {
            bg.color = backgroundColor;
        }
    }

    public void EarthPowerUp()
    {
        leafImmunity = true;
        
        jsonController.UsedPowerUp(GameMechanicsConstant.PowerUpNames.Earth);
    }

    public void LeafPowerExpired()
    {
        leafImmunity = false;
        //StartCoroutine(PowerUpFolhaAnim());
        if (isFuryMode)
        {
            bg.color = Color.red;
        }
        else
        {
            bg.color = backgroundColor;
        }
    }

    private int[] ReturnRandomIndexes(int[] indexes, int n)
    {
        int[] randomIndexes = new int[n];
        //Debug.Log(randomIndexes[0]);

        for (int i = 0; i < n; i++)
        {
            int random;
            do
            {
                random = Random.Range(1, indexes.Length + 1);
            }
            while (randomIndexes.Contains(random));
            randomIndexes[i] = random;
        }

        for (int i = 0; i < n; i++)
        {
            randomIndexes[i]--;
        }

        return randomIndexes;
    }

    public void UsePowerUp(GameMechanicsConstant.PowerUpNames name)
    {
        PowerUps pw = new PowerUps();
        for (int i = 0; i < powerUps.Count; i++)
        {
            if (powerUps[i].name == name)
            {
                pw = powerUps[i];
            }
        }

        bg.color = pw.color;

        if (pw.name == GameMechanicsConstant.PowerUpNames.Water)
        {
            WaterPowerUp();
            audioSource.PlayOneShot(audioClips[(int)Clip.water]);
        }
        else if (pw.name == GameMechanicsConstant.PowerUpNames.Air)
        {
            AirPowerUp();
            audioSource.PlayOneShot(audioClips[(int)Clip.air]);
        }
        else if (pw.name == GameMechanicsConstant.PowerUpNames.Earth)
        {
            EarthPowerUp();
            audioSource.PlayOneShot(audioClips[(int)Clip.earth]);
        }
    }

    public void SetJsonControllerReference(JsonController jsonController)
    {
        this.jsonController = jsonController;
    }
    
    IEnumerator PowerUpGeloAnim()
    {
        animator.runtimeAnimatorController = gelo;
        powerUpAnimation.SetActive(true);
        bg.color = powerUps[1].color;

        yield return new WaitForSeconds(GameMechanicsConstant.IceAnimationTimeinSeconds);
        powerUpAnimation.SetActive(false);        
    }

    IEnumerator PowerUpVentoAnim()
    {
        animator.runtimeAnimatorController = vento;
        powerUpAnimation.SetActive(true);
        bg.color = powerUps[2].color;

        yield return new WaitForSeconds(GameMechanicsConstant.WindAnimationTimeinSeconds);

        if (isFuryMode)
        {
            bg.color = Color.red;
        }
        else
        {
            bg.color = backgroundColor;
        }

        powerUpAnimation.SetActive(false);
    }

    public IEnumerator PowerUpFolhaAnim()
    {
        animator.runtimeAnimatorController = folha;
        powerUpAnimation.SetActive(true);
        bg.color = powerUps[0].color;

        yield return new WaitForSeconds(GameMechanicsConstant.LeafAnimationTimeinSeconds);

        if (isFuryMode)
        {
            bg.color = Color.red;
        }
        else
        {
            bg.color = backgroundColor;
        }

        powerUpAnimation.SetActive(false);
    }

}
