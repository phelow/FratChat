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

    [SerializeField]
    private AudioSource m_as;
    [SerializeField]
    private AudioClip m_ac;


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
    private static DialogManager ms_instance;

    // Use this for initialization
    void Start()
    {
        ms_instance = this;
        Time.timeScale = 1.0f;
        m_nextTarget = m_offscreen;
        StartCoroutine(PlayGame());
    }

    public static void LikeBlurb(DialogManager.DialogBlurb blurb)
    {
        blurb.CharacterWhoSaidIt.SetState(blurb.To);
    }

    List<string> saidBlurbs;
    List<DialogBlurb> blurbs;

    private DialogBlurb GetNextLineOfDialog()
    {
        foreach (DialogBlurb blurb in blurbs)
        {
            if ((blurb.From == DialogBlurb.DialogState.Any || blurb.CharacterWhoSaidIt.m_curState == blurb.From) && (blurb.DependentBlurb == null || saidBlurbs.Contains(blurb.DependentBlurb.GetText())) && blurb.HasNotBeenSaid())
            {
                
                blurb.Say();
                saidBlurbs.Add(blurb.GetText());
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

    private List<Character> m_characters;

    public static void PlayPop()
    {
        ms_instance.m_as.PlayOneShot(ms_instance.m_ac);
    }

    private void InitDialog()
    {
        m_characters = new List<Character>();
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


        m_characters.Add(keyboardKommander);

        m_characters.Add(smartBen);
        m_characters.Add(buildANukeFan);
        m_characters.Add(legalizeRanch);
        m_characters.Add(apatheticEngineer);
        m_characters.Add(crankyChemist);

        smartBenLines.Add(new DialogBlurb("Hey guys, get ready for this awesome joke I'm about to tell.", DialogBlurb.DialogState.Shitpost, DialogBlurb.DialogState.Shitpost, null, smartBen));
        smartBenLines.Add(new DialogBlurb("Where do you get the coolest memes from?", DialogBlurb.DialogState.Shitpost, DialogBlurb.DialogState.Dissapointment, smartBenLines[0], smartBen));
        smartBenLines.Add(new DialogBlurb("The ice meme truck!", DialogBlurb.DialogState.Shitpost, DialogBlurb.DialogState.Dissapointment, smartBenLines[1], smartBen));
        buildANukeFanLines.Add(new DialogBlurb("That joke was Radical bro!", DialogBlurb.DialogState.Shitpost, DialogBlurb.DialogState.Shitpost, smartBenLines[2], buildANukeFan));
        smartBenLines.Add(new DialogBlurb("Why didn't anyone like it. You know you can hit the like button to like it right.", DialogBlurb.DialogState.Shitpost, DialogBlurb.DialogState.Shitpost, buildANukeFanLines[0], smartBen));
        smartBenLines.Add(new DialogBlurb("Thanks guys! There's no other bros I'd rather shitpost with.", DialogBlurb.DialogState.Dissapointment, DialogBlurb.DialogState.Shitpost, null, smartBen));
        smartBenLines.Add(new DialogBlurb("You know guys if you keep giving me all these likes. I'm going to post more about the stuff you like.", DialogBlurb.DialogState.Dissapointment, DialogBlurb.DialogState.Shitpost, smartBenLines[smartBenLines.Count - 1], smartBen));
        buildANukeFanLines.Add(new DialogBlurb("I'm hyped for more awesome punzzz!", DialogBlurb.DialogState.Shitpost, DialogBlurb.DialogState.Shitpost, smartBenLines[smartBenLines.Count - 1], buildANukeFan));
        apatheticEngineerLines.Add(new DialogBlurb("Will you all shut up. I can't take this right now.", DialogBlurb.DialogState.Shitpost, DialogBlurb.DialogState.Dissapointment, buildANukeFanLines[buildANukeFanLines.Count - 1], apatheticEngineer));
        crankyChemistLines.Add(new DialogBlurb("STOP THE SHITPOSTING THIS MADNESS MUST END!!", DialogBlurb.DialogState.Shitpost, DialogBlurb.DialogState.Dissapointment, apatheticEngineerLines[apatheticEngineerLines.Count - 1], crankyChemist));
        legalizeRanchLines.Add(new DialogBlurb("Ay, All yalz, You needz tah chill, Theze memes are dank af.", DialogBlurb.DialogState.Shitpost, DialogBlurb.DialogState.Dissapointment, crankyChemistLines[crankyChemistLines.Count - 1], legalizeRanch));
        legalizeRanchLines.Add(new DialogBlurb("Clearly, teh age of shitpost will reign supreme!", DialogBlurb.DialogState.Dissapointment, DialogBlurb.DialogState.Shitpost, crankyChemistLines[crankyChemistLines.Count - 1], legalizeRanch));
        keyboardKommanderLines.Add(new DialogBlurb("Hey guys, I'm going AWOL for a bit. Sorry", DialogBlurb.DialogState.Shitpost, DialogBlurb.DialogState.Shitpost, legalizeRanchLines[legalizeRanchLines.Count - 1], keyboardKommander));
        keyboardKommanderLines.Add(new DialogBlurb("--Keyboard Kommander has left the group--", DialogBlurb.DialogState.Shitpost, DialogBlurb.DialogState.Shitpost, keyboardKommanderLines[keyboardKommanderLines.Count - 1], keyboardKommander));
        smartBenLines.Add(new DialogBlurb("I bet if we all agree on what happened he'll come back. I wish he was here right now though.", DialogBlurb.DialogState.Any, DialogBlurb.DialogState.Dissapointment, keyboardKommanderLines[keyboardKommanderLines.Count - 1], smartBen));
        smartBenLines.Add(new DialogBlurb("I wonder why he left the group? He's been really flaky lately.", DialogBlurb.DialogState.Shitpost, DialogBlurb.DialogState.Dissapointment, keyboardKommanderLines[keyboardKommanderLines.Count - 1], smartBen));
        buildANukeFanLines.Add(new DialogBlurb("@KeyboardKommander is a MEGA hardcore student. He probably just needs to study.", DialogBlurb.DialogState.Any, DialogBlurb.DialogState.Academics, keyboardKommanderLines[keyboardKommanderLines.Count - 1], buildANukeFan));
        crankyChemistLines.Add(new DialogBlurb("HE'S PROBABLY STILL WHINING OVER HIS LAST BREAKUP, THE LITTLE BITCH!", DialogBlurb.DialogState.Any, DialogBlurb.DialogState.Breakup, keyboardKommanderLines[keyboardKommanderLines.Count - 1], crankyChemist));
        legalizeRanchLines.Add(new DialogBlurb("Ay, I'll just send meme's bros. That alwayz putz him in a better mood yo.", DialogBlurb.DialogState.Any, DialogBlurb.DialogState.Shitpost, keyboardKommanderLines[keyboardKommanderLines.Count - 1], legalizeRanch));
        legalizeRanchLines.Add(new DialogBlurb("He didn't respond. I better keep sending him memes until he feels better", DialogBlurb.DialogState.Shitpost, DialogBlurb.DialogState.Shitpost, legalizeRanchLines[keyboardKommanderLines.Count - 1], legalizeRanch));
        apatheticEngineerLines.Add(new DialogBlurb("Meh, there's not anything we can do to make him feel better", DialogBlurb.DialogState.Any, DialogBlurb.DialogState.Dissapointment, keyboardKommanderLines[keyboardKommanderLines.Count - 1], apatheticEngineer));
        crankyChemistLines.Add(new DialogBlurb("I'VE NEVER REALLY RESPECTED WILL TO BEGIN WITH!", DialogBlurb.DialogState.Any, DialogBlurb.DialogState.Dissapointment, keyboardKommanderLines[keyboardKommanderLines.Count - 1], crankyChemist));
        crankyChemistLines.Add(new DialogBlurb("I STILL THINK IT'S SO FUNNY THAT WILL LEFT THE GROUPME. WHAT A SCRUB. I WONDER IF SOMEONE IN HIS FAMILY DIED.", DialogBlurb.DialogState.Dissapointment, DialogBlurb.DialogState.Loss, keyboardKommanderLines[keyboardKommanderLines.Count - 1], crankyChemist));
        crankyChemistLines.Add(new DialogBlurb("YEAH I BET HIS WHOLE FAMILY IS DEAD. FOREVER. THIS IS PRETTY FUNNY TO ME. LOL WHAT A SCRUB.", DialogBlurb.DialogState.Loss, DialogBlurb.DialogState.Loss, crankyChemistLines[crankyChemistLines.Count - 1], crankyChemist));
        smartBenLines.Add(new DialogBlurb("@CrankyChemist, sounds like your just MADDDDD!!!!", DialogBlurb.DialogState.Any, DialogBlurb.DialogState.Shitpost, crankyChemistLines[crankyChemistLines.Count - 1], smartBen));
        smartBenLines.Add(new DialogBlurb("I hope @KeyboardKommander is okay though", DialogBlurb.DialogState.Any, DialogBlurb.DialogState.Dissapointment, smartBenLines[smartBenLines.Count - 1], smartBen));
        buildANukeFanLines.Add(new DialogBlurb("I wonder if he's dead", DialogBlurb.DialogState.Any, DialogBlurb.DialogState.Dissapointment, keyboardKommanderLines[keyboardKommanderLines.Count - 1], buildANukeFan));
        buildANukeFanLines.Add(new DialogBlurb("How's he going to make Build a Nuke 2 if he's dead.", DialogBlurb.DialogState.Dissapointment, DialogBlurb.DialogState.Dissapointment, keyboardKommanderLines[keyboardKommanderLines.Count - 1], buildANukeFan));
        buildANukeFanLines.Add(new DialogBlurb("Who's going to do everything if @KeyboardKommander is gone. Do you think he left because of his breakup?", DialogBlurb.DialogState.Dissapointment, DialogBlurb.DialogState.Breakup, keyboardKommanderLines[keyboardKommanderLines.Count - 1], buildANukeFan));
        buildANukeFanLines.Add(new DialogBlurb("I guess we're never going to see him again because of that stupid bitch. Maybe he'll have time to focus on his academics now.", DialogBlurb.DialogState.Breakup, DialogBlurb.DialogState.Academics, keyboardKommanderLines[keyboardKommanderLines.Count - 1], buildANukeFan));
        buildANukeFanLines.Add(new DialogBlurb("Will was really struggling with his academics. Maybe that's why he never made Build a Nuke 2.", DialogBlurb.DialogState.Academics, DialogBlurb.DialogState.Dissapointment, keyboardKommanderLines[keyboardKommanderLines.Count - 1], buildANukeFan));
        buildANukeFanLines.Add(new DialogBlurb("I feel like I'm missing out on something without a sequel to build a nuke.", DialogBlurb.DialogState.Any, DialogBlurb.DialogState.Loss, keyboardKommanderLines[keyboardKommanderLines.Count - 1], buildANukeFan));
        buildANukeFanLines.Add(new DialogBlurb("I wonder if @KeyboardKommander lost someone close to him.", DialogBlurb.DialogState.Loss, DialogBlurb.DialogState.Loss, keyboardKommanderLines[keyboardKommanderLines.Count - 1], buildANukeFan));
        smartBenLines.Add(new DialogBlurb("Something must be wrong @KeyboardKommander loves shitposting.",DialogBlurb.DialogState.Any,DialogBlurb.DialogState.Dissapointment, keyboardKommanderLines[keyboardKommanderLines.Count - 1],smartBen));
        smartBenLines.Add(new DialogBlurb("I feel like @KeyboardKommander always lets us down. He's too focused on grades.", DialogBlurb.DialogState.Dissapointment, DialogBlurb.DialogState.Academics, keyboardKommanderLines[keyboardKommanderLines.Count - 1], smartBen));
        smartBenLines.Add(new DialogBlurb("If he wasn't so focused on academics maybe his girlfriend wouldn't have left him.", DialogBlurb.DialogState.Academics, DialogBlurb.DialogState.Breakup, keyboardKommanderLines[keyboardKommanderLines.Count - 1], smartBen));
        smartBenLines.Add(new DialogBlurb("I heard that someone in @KeyboardKommander's family passed away.", DialogBlurb.DialogState.Any, DialogBlurb.DialogState.Loss, keyboardKommanderLines[keyboardKommanderLines.Count - 1], smartBen));
        buildANukeFanLines.Add(new DialogBlurb("I feel so dejected without a build a nuke sequel. I bet if we agree on what was wrong he'd come back.", DialogBlurb.DialogState.Dissapointment, DialogBlurb.DialogState.Dissapointment, keyboardKommanderLines[keyboardKommanderLines.Count - 1], buildANukeFan));
        apatheticEngineerLines.Add(new DialogBlurb("This groupme is better without @KeyboardKommander's shitposting. Still I hope he's not stressing about schoolwork", DialogBlurb.DialogState.Any, DialogBlurb.DialogState.Academics, keyboardKommanderLines[keyboardKommanderLines.Count - 1], apatheticEngineer));
        crankyChemistLines.Add(new DialogBlurb("@KeyboardKommander IS FINALLY GONE. WE BETTER NOT AGREE ON WHAT HAPPENED OR HE'LL COME BACK AND COMPLAIN ABOUT HIS EX AGAIN!", DialogBlurb.DialogState.Any, DialogBlurb.DialogState.Breakup, keyboardKommanderLines[keyboardKommanderLines.Count - 1], crankyChemist));
        crankyChemistLines.Add(new DialogBlurb("Ayyy you guyzz where's the meme masta when you need em. Probably failin his clazzez.", DialogBlurb.DialogState.Any, DialogBlurb.DialogState.Academics, keyboardKommanderLines[keyboardKommanderLines.Count - 1], legalizeRanch));
        crankyChemistLines.Add(new DialogBlurb("Bring back the meme masta. We all need to agree on why he's gone.", DialogBlurb.DialogState.Any, DialogBlurb.DialogState.Shitpost, keyboardKommanderLines[keyboardKommanderLines.Count - 1], legalizeRanch));

        buildANukeFanLines.Add(new DialogBlurb("If we all agree on why he left @KeyboardKommander can make more of the terrible games I love.", DialogBlurb.DialogState.Any, DialogBlurb.DialogState.Shitpost, keyboardKommanderLines[keyboardKommanderLines.Count - 1], buildANukeFan));
        buildANukeFanLines.Add(new DialogBlurb("He's probably just busy working on Monstinder", DialogBlurb.DialogState.Any, DialogBlurb.DialogState.Academics, keyboardKommanderLines[keyboardKommanderLines.Count - 1], buildANukeFan));
        buildANukeFanLines.Add(new DialogBlurb("Maybe if @KeyboardKommander focused less on his games he'd have better grades.", DialogBlurb.DialogState.Any, DialogBlurb.DialogState.Academics, keyboardKommanderLines[keyboardKommanderLines.Count - 1], buildANukeFan));
        buildANukeFanLines.Add(new DialogBlurb("I wonder why @KeyboardKommander is doing so poorly in his classes at the moment", DialogBlurb.DialogState.Academics, DialogBlurb.DialogState.Academics, keyboardKommanderLines[keyboardKommanderLines.Count - 1], buildANukeFan));
        buildANukeFanLines.Add(new DialogBlurb("I always thought @KeyboardKommander was smart. I guess I was wrong", DialogBlurb.DialogState.Academics, DialogBlurb.DialogState.Academics, keyboardKommanderLines[keyboardKommanderLines.Count - 1], buildANukeFan));
        buildANukeFanLines.Add(new DialogBlurb("I never even knew Game Design was a major", DialogBlurb.DialogState.Academics, DialogBlurb.DialogState.Academics, keyboardKommanderLines[keyboardKommanderLines.Count - 1], buildANukeFan));
        buildANukeFanLines.Add(new DialogBlurb("I can't believe @KeyboardKommander gets paid to make videogames all day", DialogBlurb.DialogState.Academics, DialogBlurb.DialogState.Academics, keyboardKommanderLines[keyboardKommanderLines.Count - 1], buildANukeFan));
        buildANukeFanLines.Add(new DialogBlurb("I'm so sad I haven't heard from @KeyboardKommander", DialogBlurb.DialogState.Any, DialogBlurb.DialogState.Dissapointment, keyboardKommanderLines[keyboardKommanderLines.Count - 1], buildANukeFan));
        buildANukeFanLines.Add(new DialogBlurb("Why is @KeyboardKommander such a dissapointment.", DialogBlurb.DialogState.Any, DialogBlurb.DialogState.Dissapointment, keyboardKommanderLines[keyboardKommanderLines.Count - 1], buildANukeFan));
        buildANukeFanLines.Add(new DialogBlurb("I'm dissapointed @KeyboardKommander is stil missing.", DialogBlurb.DialogState.Any, DialogBlurb.DialogState.Dissapointment, keyboardKommanderLines[keyboardKommanderLines.Count - 1], buildANukeFan));
        buildANukeFanLines.Add(new DialogBlurb("Who is going to take notes on the meeting now.", DialogBlurb.DialogState.Any, DialogBlurb.DialogState.Dissapointment, keyboardKommanderLines[keyboardKommanderLines.Count - 1], buildANukeFan));
        buildANukeFanLines.Add(new DialogBlurb("Maybe we'll never get build a nuke two because he lost a family member.", DialogBlurb.DialogState.Dissapointment, DialogBlurb.DialogState.Loss, keyboardKommanderLines[keyboardKommanderLines.Count - 1], buildANukeFan));
        buildANukeFanLines.Add(new DialogBlurb("Who is going to make memes now", DialogBlurb.DialogState.Dissapointment, DialogBlurb.DialogState.Shitpost, keyboardKommanderLines[keyboardKommanderLines.Count - 1], buildANukeFan));

        buildANukeFanLines.Add(new DialogBlurb("I'm dissapointed that my favorite game developer left", DialogBlurb.DialogState.Any, DialogBlurb.DialogState.Dissapointment, keyboardKommanderLines[keyboardKommanderLines.Count - 1], buildANukeFan));
        buildANukeFanLines.Add(new DialogBlurb("I still have trouble believing @KeyboardKommander's relationship status", DialogBlurb.DialogState.Any, DialogBlurb.DialogState.Breakup, keyboardKommanderLines[keyboardKommanderLines.Count - 1], buildANukeFan));
        buildANukeFanLines.Add(new DialogBlurb("I talked to @KeyboardKommander someone in his family died.", DialogBlurb.DialogState.Any, DialogBlurb.DialogState.Loss, keyboardKommanderLines[keyboardKommanderLines.Count - 1], buildANukeFan));
        buildANukeFanLines.Add(new DialogBlurb("I haven't seen @KeyboardKommander studying at all this semester,", DialogBlurb.DialogState.Any, DialogBlurb.DialogState.Academics, keyboardKommanderLines[keyboardKommanderLines.Count - 1], buildANukeFan));



        smartBenLines.Add(new DialogBlurb("Look I know we're all being serious right now but how about we just shitpost.", DialogBlurb.DialogState.Any, DialogBlurb.DialogState.Shitpost, keyboardKommanderLines[keyboardKommanderLines.Count - 1], smartBen));
        smartBenLines.Add(new DialogBlurb("Does anyone need a ride? Just kidding psych.", DialogBlurb.DialogState.Any, DialogBlurb.DialogState.Dissapointment, keyboardKommanderLines[keyboardKommanderLines.Count - 1], smartBen));
        smartBenLines.Add(new DialogBlurb("This chat needs some memes.", DialogBlurb.DialogState.Any, DialogBlurb.DialogState.Dissapointment, keyboardKommanderLines[keyboardKommanderLines.Count - 1], smartBen));
        buildANukeFanLines.Add(new DialogBlurb("That's a clever comment, but instead of addressing it. I'm going to shitpost", DialogBlurb.DialogState.Any, DialogBlurb.DialogState.Shitpost, keyboardKommanderLines[keyboardKommanderLines.Count - 1], buildANukeFan));
        smartBenLines.Add(new DialogBlurb("Hey deos anyone want to like my comment.", DialogBlurb.DialogState.Shitpost, DialogBlurb.DialogState.Shitpost, keyboardKommanderLines[keyboardKommanderLines.Count - 1], smartBen));
        smartBenLines.Add(new DialogBlurb("Thinking on my feet is not my best strength.", DialogBlurb.DialogState.Shitpost, DialogBlurb.DialogState.Shitpost, keyboardKommanderLines[keyboardKommanderLines.Count - 1], smartBen));

        smartBenLines.Add(new DialogBlurb("I can't believe @KeyboardKommander was dating his female roommate.", DialogBlurb.DialogState.Any, DialogBlurb.DialogState.Breakup, keyboardKommanderLines[keyboardKommanderLines.Count - 1], smartBen));
        smartBenLines.Add(new DialogBlurb("See whenever I type stuff onto the groupme. I try to shitpost then I remember how much of a cuckold @KeyboardKommander is.", DialogBlurb.DialogState.Any, DialogBlurb.DialogState.Breakup, keyboardKommanderLines[keyboardKommanderLines.Count - 1], smartBen));
        smartBenLines.Add(new DialogBlurb("I just randomly decided to start talking about @KeyboardKommander's breakup.", DialogBlurb.DialogState.Any, DialogBlurb.DialogState.Breakup, keyboardKommanderLines[keyboardKommanderLines.Count - 1], smartBen));
        buildANukeFanLines.Add(new DialogBlurb("I think @KeyboardKommander's girlfriend left him because his haircut is lame", DialogBlurb.DialogState.Breakup, DialogBlurb.DialogState.Breakup, keyboardKommanderLines[keyboardKommanderLines.Count - 1], buildANukeFan));
        smartBenLines.Add(new DialogBlurb("Hey deos anyone want to like my comment about @KeyboardKommander's breakup.", DialogBlurb.DialogState.Breakup, DialogBlurb.DialogState.Breakup, keyboardKommanderLines[keyboardKommanderLines.Count - 1], smartBen));

        smartBenLines.Add(new DialogBlurb("I'm concerned about @KeyboardKommander I heard someone in his family died.", DialogBlurb.DialogState.Any, DialogBlurb.DialogState.Loss, keyboardKommanderLines[keyboardKommanderLines.Count - 1], smartBen));
        smartBenLines.Add(new DialogBlurb("@KeyboardKommander loses things a lot. Like family members", DialogBlurb.DialogState.Any, DialogBlurb.DialogState.Loss, keyboardKommanderLines[keyboardKommanderLines.Count - 1], smartBen));
        smartBenLines.Add(new DialogBlurb("I wonder if @KeyboardKommander left because of the loss of a relatie.", DialogBlurb.DialogState.Dissapointment, DialogBlurb.DialogState.Loss, keyboardKommanderLines[keyboardKommanderLines.Count - 1], smartBen));

        smartBenLines.Add(new DialogBlurb("I wonder if @KeyboardKommander failed out because of oceanography.", DialogBlurb.DialogState.Any, DialogBlurb.DialogState.Academics, keyboardKommanderLines[keyboardKommanderLines.Count - 1], smartBen));
        smartBenLines.Add(new DialogBlurb("@KeyboardKommander did so bad in oceanography they failed him for all his oher classes.", DialogBlurb.DialogState.Academics, DialogBlurb.DialogState.Academics, keyboardKommanderLines[keyboardKommanderLines.Count - 1], smartBen));
        smartBenLines.Add(new DialogBlurb("I wonder if @KeyboardKommander left because of the loss of Academics.", DialogBlurb.DialogState.Dissapointment, DialogBlurb.DialogState.Academics, keyboardKommanderLines[keyboardKommanderLines.Count - 1], smartBen));
        smartBenLines.Add(new DialogBlurb("I thought @KeyboardKommander was smart then I realized he was just Autistic.", DialogBlurb.DialogState.Shitpost, DialogBlurb.DialogState.Academics, keyboardKommanderLines[keyboardKommanderLines.Count - 1], smartBen));

        legalizeRanchLines.Add(new DialogBlurb("Ay, @KeyboardKommander wuz such a good scholar. What happened.", DialogBlurb.DialogState.Any, DialogBlurb.DialogState.Academics, keyboardKommanderLines[keyboardKommanderLines.Count - 1], legalizeRanch));
        legalizeRanchLines.Add(new DialogBlurb("@KeyboardKommander wut a womanizer.", DialogBlurb.DialogState.Any, DialogBlurb.DialogState.Breakup, keyboardKommanderLines[keyboardKommanderLines.Count - 1], legalizeRanch));
        legalizeRanchLines.Add(new DialogBlurb("I heard abou the car crash. I hope he's coping alright.", DialogBlurb.DialogState.Any, DialogBlurb.DialogState.Loss, keyboardKommanderLines[keyboardKommanderLines.Count - 1], legalizeRanch));

        apatheticEngineerLines.Add(new DialogBlurb("@KeyboardKommander actually likes school. How is he failing", DialogBlurb.DialogState.Any, DialogBlurb.DialogState.Academics, keyboardKommanderLines[keyboardKommanderLines.Count - 1], apatheticEngineer));
        apatheticEngineerLines.Add(new DialogBlurb("@KeyboardKommander is worse at talking to girls than I am.", DialogBlurb.DialogState.Any, DialogBlurb.DialogState.Breakup, keyboardKommanderLines[keyboardKommanderLines.Count - 1], apatheticEngineer));
        apatheticEngineerLines.Add(new DialogBlurb("@KeyboardKommander seems so blue. I wonder what's wrong.", DialogBlurb.DialogState.Any, DialogBlurb.DialogState.Loss, keyboardKommanderLines[keyboardKommanderLines.Count - 1], apatheticEngineer));


        crankyChemistLines.Add(new DialogBlurb("VIDEOGAMES ISN'T A REAL MAJOR.", DialogBlurb.DialogState.Any, DialogBlurb.DialogState.Academics, keyboardKommanderLines[keyboardKommanderLines.Count - 1], crankyChemist));
        crankyChemistLines.Add(new DialogBlurb("TALKING TO GIRLS IS HARD.", DialogBlurb.DialogState.Any, DialogBlurb.DialogState.Breakup, keyboardKommanderLines[keyboardKommanderLines.Count - 1], crankyChemist));
        crankyChemistLines.Add(new DialogBlurb("I'M GLAD THAT THAT SCRUB IS SO SAD.", DialogBlurb.DialogState.Any, DialogBlurb.DialogState.Loss, keyboardKommanderLines[keyboardKommanderLines.Count - 1], crankyChemist));

        apatheticEngineerLines.Add(new DialogBlurb("Yall need to shut it with all the shitpost. I’m trying to get drunk and my phone keeps buzzing, ugh..", DialogBlurb.DialogState.Shitpost, DialogBlurb.DialogState.Dissapointment, keyboardKommanderLines[keyboardKommanderLines.Count - 1], apatheticEngineer));

        apatheticEngineerLines.Add(new DialogBlurb("I can’t believe you idiots actually think someone in our house is studying. Also, why am I even in this game, why am I hungover when I haven’t even finished drinking, and what day is it, Please kill me...", DialogBlurb.DialogState.Academics, DialogBlurb.DialogState.Academics, keyboardKommanderLines[keyboardKommanderLines.Count - 1], apatheticEngineer));


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
        saidBlurbs = new List<string>();
        int month = 10;
        int day = 24;
        int year = 2016;
        int hour = 4;
        int minutes = 26;
        bool endgame = false;
        while (!(endgame))
        {
            minutes += 1 + Random.Range(2, 10);

            if (minutes > 59)
            {
                hour++;
                minutes = minutes % 59;
            }

            if (hour > 12)
            {
                hour = hour % 12;
                day++;
            }

            if (day > 30)
            {
                day = day % 30;
                month++;
            }

            if (month > 12)
            {
                month = month % 12;
                year++;
            }


            GameObject go_newDialogBox = GameObject.Instantiate(mp_dialogBox, m_spawnLocation.transform.position, m_spawnLocation.transform.rotation, m_gameCanvas.transform) as GameObject;

            DialogBlurb nextBlurb = null;
            while (nextBlurb == null)
            {
                int breakup = 0;
                int loss = 0;
                int academics = 0;

                foreach (Character c in m_characters)
                {
                    if (c.m_curState == DialogBlurb.DialogState.Loss)
                    {
                        loss++;
                    }
                    else if (c.m_curState == DialogBlurb.DialogState.Breakup)
                    {
                        breakup++;
                    }
                    else if (c.m_curState == DialogBlurb.DialogState.Academics)
                    {
                        academics++;
                    }
                }

                if (breakup == 4 || loss == 4 || academics == 4)
                {
                    nextBlurb = (new DialogBlurb("Hey guys I'm back.", DialogBlurb.DialogState.Any, DialogBlurb.DialogState.Any, null, m_characters[0]));
                    endgame = true;
                }
                else
                {
                    nextBlurb = GetNextLineOfDialog();
                }

                yield return new WaitForEndOfFrame();
            }

            go_newDialogBox.GetComponent<DialogLine>().Setup(nextBlurb, "" + hour + ":" + minutes);


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
