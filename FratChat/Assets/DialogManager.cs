using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DialogManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mp_dialogBox;

    [SerializeField]
    private GameObject m_spawnLocation;

    [SerializeField]
    private GameObject m_gameCanvas;

    [SerializeField]
    private GameObject m_offscreen;
    [SerializeField]
    private GameObject m_top;

    private List<GameObject> m_boxes;
    private GameObject m_nextTarget;

    public class DialogBlurb
    {
        string m_text;
        public enum DialogState
        {
            Dissapointment,
            Academics,
            Breakup,
            Loss,
            Shitpost,
            Any
        }

        private DialogState m_from;
        private DialogState m_to;
        private bool m_said;
        private DialogBlurb m_dependentOnLine;
        private Character m_character;

        public Character CharacterWhoSaidIt
        {
            get
            {
                return m_character;
            }
        }

        public DialogState From
        {
            get
            {
                return m_from;
            }
        }

        public DialogState To
        {
            get
            {
                return m_to;
            }
        }

        public bool HasNotBeenSaid()
        {
            return m_said == false;
        }

        public string GetText()
        {
            return m_text;
        }

        public DialogBlurb(string text, DialogState from, DialogState to, DialogBlurb dependentOnLine, Character character)
        {
            m_text = text;
            m_from = from;
            m_to = to;
            m_dependentOnLine = dependentOnLine;
            m_said = false;
            m_character = character;
        }

        public DialogBlurb DependentBlurb
        {
            get
            {
                return m_dependentOnLine;
            }
        }

        public void Say()
        {
            m_said = true;
        }
    }

    public class Character
    {
        private List<DialogBlurb> m_lines;
        private string m_name;
        private Sprite m_profilePicture;

        public DialogBlurb.DialogState m_curState;

        public Sprite ProfilePicture
        {
            get
            {
                return m_profilePicture;
            }
        }

        public Character(string name, List<DialogBlurb> lines, Sprite profilePicture, DialogBlurb.DialogState startingState)
        {
            m_name = name;
            m_lines = lines;
            m_profilePicture = profilePicture;

            m_curState = startingState;
        }

        public void SetState(DialogBlurb.DialogState setState)
        {
            m_curState = setState;
        }

        public string Name
        {
            get
            {
                return m_name;
            }
        }

        public DialogBlurb.DialogState State
        {
            get
            {
                return m_curState;

            }
        }
    }


    // Use this for initialization
    void Start()
    {
        m_nextTarget = m_offscreen;
        StartCoroutine(PlayGame());
    }

    public static void LikeBlurb(DialogManager.DialogBlurb blurb)
    {
        blurb.CharacterWhoSaidIt.SetState(blurb.To);
    }

    List<DialogBlurb> saidBlurbs;
    List<DialogBlurb> blurbs;

    private DialogBlurb GetNextLineOfDialog()
    {
        foreach(DialogBlurb blurb in blurbs)
        {
            if((blurb.CharacterWhoSaidIt.m_curState== DialogBlurb.DialogState.Any || blurb.CharacterWhoSaidIt.m_curState == blurb.From) && (saidBlurbs.Contains(blurb.DependentBlurb) || blurb.DependentBlurb == null) && blurb.HasNotBeenSaid())
            {
                saidBlurbs.Add(blurb);
                blurb.Say();

                return blurb;
            }
        }

        return null;

    }
    
    [SerializeField]
    private Sprite m_keyboardKommanderSprite;

    [SerializeField]
    private Sprite m_buildANukeFanSprite;

    [SerializeField]
    private Sprite m_smartBenSprite;

    [SerializeField]
    private Sprite m_legalizeRanchSprite;

    [SerializeField]
    private Sprite m_apatheticEngineerSprite;

    [SerializeField]
    private Sprite m_crankyGermanSprite;

    private void InitDialog()
    {
        List<DialogBlurb> buildANukeFanLines = new List<DialogBlurb>();
        List<DialogBlurb> keyboardKommanderLines = new List<DialogBlurb>();
        List<DialogBlurb> smartBenLines = new List<DialogBlurb>();
        List<DialogBlurb> legalizeRanchLines = new List<DialogBlurb>();
        List<DialogBlurb> apatheticEngineerLines = new List<DialogBlurb>();
        List<DialogBlurb> crankyChemistLines = new List<DialogBlurb>();



        blurbs = new List<DialogBlurb>();

        Character smartBen = new Character("Smart Ben", smartBenLines, m_smartBenSprite, DialogBlurb.DialogState.Shitpost);




        Character buildANukeFan = new Character("Build A Nuke #1 Fan", buildANukeFanLines, m_buildANukeFanSprite, DialogBlurb.DialogState.Shitpost);


        Character keyboardKommander = new Character("Keyboard Kommander", keyboardKommanderLines, m_keyboardKommanderSprite, DialogBlurb.DialogState.Shitpost);



        Character legalizeRanch = new Character("Legalize Ranch", legalizeRanchLines, m_legalizeRanchSprite, DialogBlurb.DialogState.Shitpost);



        Character apatheticEngineer = new Character("Apathetic Engineer", apatheticEngineerLines, m_apatheticEngineerSprite, DialogBlurb.DialogState.Shitpost);




        Character crankyChemist = new Character("Cranky Chemist", crankyChemistLines, m_crankyGermanSprite, DialogBlurb.DialogState.Shitpost);


        smartBenLines.Add(new DialogBlurb("Hey guys, get ready for this awesome joke I'm about to tell.", DialogBlurb.DialogState.Shitpost, DialogBlurb.DialogState.Shitpost, null, smartBen));
        smartBenLines.Add(new DialogBlurb("Where do you get the coolest memes from?", DialogBlurb.DialogState.Shitpost, DialogBlurb.DialogState.Dissapointment, smartBenLines[0], smartBen));
        smartBenLines.Add(new DialogBlurb("The ice meme truck!", DialogBlurb.DialogState.Shitpost, DialogBlurb.DialogState.Dissapointment, smartBenLines[1], smartBen));
        buildANukeFanLines.Add(new DialogBlurb("That joke was Radical bro!", DialogBlurb.DialogState.Shitpost, DialogBlurb.DialogState.Shitpost, smartBenLines[2], buildANukeFan));
        smartBenLines.Add(new DialogBlurb("Why didn't anyone like it. You know you can hit the like button to like it right.", DialogBlurb.DialogState.Shitpost, DialogBlurb.DialogState.Shitpost, buildANukeFanLines[0], smartBen));
        smartBenLines.Add(new DialogBlurb("Thanks guys! There's no other bros I'd rather shitpost with.", DialogBlurb.DialogState.Dissapointment, DialogBlurb.DialogState.Shitpost, null, smartBen));
        smartBenLines.Add(new DialogBlurb("You know guys if you keep giving me all these likes. I'm going to post more about the stuff you like.", DialogBlurb.DialogState.Dissapointment, DialogBlurb.DialogState.Shitpost, smartBenLines[smartBenLines.Count-1], smartBen));
        buildANukeFanLines.Add(new DialogBlurb("I'm hyped for more awesome punzzz!", DialogBlurb.DialogState.Shitpost, DialogBlurb.DialogState.Shitpost, smartBenLines[smartBenLines.Count - 1], buildANukeFan));
        apatheticEngineerLines.Add(new DialogBlurb("Will you all shut up. I can't take this right now.", DialogBlurb.DialogState.Shitpost, DialogBlurb.DialogState.Dissapointment, buildANukeFanLines[buildANukeFanLines.Count - 1], apatheticEngineer));
        crankyChemistLines.Add(new DialogBlurb("STOP THE SHITPOSTING THIS MADNESS MUST END!!", DialogBlurb.DialogState.Shitpost, DialogBlurb.DialogState.Dissapointment, apatheticEngineerLines[apatheticEngineerLines.Count - 1], crankyChemist));
        legalizeRanchLines.Add(new DialogBlurb("Ay, All yalz, You needz tah chill, Theze memes are dank af", DialogBlurb.DialogState.Shitpost, DialogBlurb.DialogState.Dissapointment, crankyChemistLines[crankyChemistLines.Count - 1], legalizeRanch));
        legalizeRanchLines.Add(new DialogBlurb("Clearly, teh age of shitpost will reign supreme!", DialogBlurb.DialogState.Dissapointment, DialogBlurb.DialogState.Shitpost, crankyChemistLines[crankyChemistLines.Count - 1], legalizeRanch));
        keyboardKommanderLines.Add(new DialogBlurb("Hey guys, I'm going AWOL for a bit. Sorry", DialogBlurb.DialogState.Shitpost, DialogBlurb.DialogState.Shitpost, legalizeRanchLines[legalizeRanchLines.Count - 1], keyboardKommander));
        keyboardKommanderLines.Add(new DialogBlurb("--Keyboard Kommander has left the group--", DialogBlurb.DialogState.Shitpost, DialogBlurb.DialogState.Shitpost, keyboardKommanderLines[keyboardKommanderLines.Count - 1], keyboardKommander));
        smartBenLines.Add(new DialogBlurb("I wonder why he left the group? He's been really flaky lately.", DialogBlurb.DialogState.Shitpost, DialogBlurb.DialogState.Dissapointment, keyboardKommanderLines[keyboardKommanderLines.Count-1], smartBen));
        buildANukeFanLines.Add(new DialogBlurb("@KeyboardKommander is a MEGA hardcore student. He probably just needs to study.", DialogBlurb.DialogState.Any, DialogBlurb.DialogState.Academics, keyboardKommanderLines[keyboardKommanderLines.Count - 1], buildANukeFan));
        crankyChemistLines.Add(new DialogBlurb("HE'S PROBABLY STILL WHINING OVER HIS LAST BREAKUP, THE LITTLE BITCH!", DialogBlurb.DialogState.Any, DialogBlurb.DialogState.Breakup, keyboardKommanderLines[keyboardKommanderLines.Count - 1], crankyChemist));


        blurbs.AddRange(smartBenLines);
        blurbs.AddRange(buildANukeFanLines);
        blurbs.AddRange(keyboardKommanderLines);
        blurbs.AddRange(legalizeRanchLines);
        blurbs.AddRange(apatheticEngineerLines);
        blurbs.AddRange(crankyChemistLines);
    }

    private IEnumerator PlayGame()
    {
        InitDialog();
        m_boxes = new List<GameObject>();
        saidBlurbs = new List<DialogBlurb>();
        int month = 10;
        int day = 24;
        int year = 2016;
        int hour = 4;
        int minutes = 26;

        while (true)
        {
            minutes += 1 + Random.Range(2,10);

            if(minutes > 59)
            {
                hour++;
                minutes = minutes % 59;
            }

            if(hour > 12)
            {
                hour = hour % 12;
                day++;
            }

            if(day > 30)
            {
                day = day % 30;
                month++;
            }

            if(month > 12)
            {
                month = month % 12;
                year++;
            }


            GameObject go_newDialogBox = GameObject.Instantiate(mp_dialogBox, m_spawnLocation.transform.position, m_spawnLocation.transform.rotation, m_gameCanvas.transform) as GameObject;

            DialogBlurb nextBlurb = null;
            while(nextBlurb == null)
            {
                nextBlurb = GetNextLineOfDialog();
                yield return new WaitForEndOfFrame();
            }

            go_newDialogBox.GetComponent<DialogLine>().Setup(nextBlurb, "" + hour + ":" + minutes );


            if (m_boxes.Count == 4)
            {
                m_boxes[0].GetComponent<DialogLine>().SetTarget(m_offscreen);
                m_boxes.Remove(m_boxes[0]);
            }

            m_boxes.Add(go_newDialogBox);

            m_boxes[0].GetComponent<DialogLine>().SetTarget(m_top);

            for (int i = 1; i < m_boxes.Count; i++)
            {
                m_boxes[i].GetComponent<DialogLine>().SetTarget(m_boxes[i - 1].GetComponent<DialogLine>().Target);
            }


            yield return new WaitForSeconds(5.0f);
        }
    }


    private IEnumerator SpawningTest()
    {
        m_boxes = new List<GameObject>();
        while (true)
        {
            GameObject go_newDialogBox = GameObject.Instantiate(mp_dialogBox, m_spawnLocation.transform.position, m_spawnLocation.transform.rotation, m_gameCanvas.transform) as GameObject;

            go_newDialogBox.GetComponent<DialogLine>();


            if (m_boxes.Count == 4)
            {
                m_boxes[0].GetComponent<DialogLine>().SetTarget(m_offscreen);
                m_boxes.Remove(m_boxes[0]);
            }

            m_boxes.Add(go_newDialogBox);

            m_boxes[0].GetComponent<DialogLine>().SetTarget(m_top);

            for (int i = 1; i < m_boxes.Count; i++)
            {
                m_boxes[i].GetComponent<DialogLine>().SetTarget(m_boxes[i - 1].GetComponent<DialogLine>().Target);
            }


            yield return new WaitForSeconds(5.0f);
        }
    }

}
