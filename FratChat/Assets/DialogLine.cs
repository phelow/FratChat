using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogLine : MonoBehaviour {
    [SerializeField]
    private Text m_name;
    [SerializeField]
    private Image m_sprite;
    [SerializeField]
    private Text m_time;

    [SerializeField]
    private Text m_text;
    [SerializeField]
    private Text m_likeButtonText;

    [SerializeField]
    private GameObject m_target;

    private DialogManager.DialogBlurb m_blurb;

    [SerializeField]
    private Button m_button;

    public GameObject Target
    {
        get { return m_target; }
    }

    public void Setup(DialogManager.DialogBlurb text,string time)
    {
        m_text.text = text.GetText();
        m_sprite.sprite = text.CharacterWhoSaidIt.ProfilePicture;
        m_name.text = text.CharacterWhoSaidIt.Name;
        m_time.text = time;
        m_blurb = text;
    }

    public void SetTarget(GameObject target)
    {

        StartCoroutine(LerpToTarget(target));
    }

    public void LikeClicked()
    {
        DialogManager.PlayPop();
        m_button.image.color = Color.red;
        m_blurb.CharacterWhoSaidIt.SetState(m_blurb.To);
        m_button.interactable = false;
        DialogManager.LikeBlurb(m_blurb);
        StartCoroutine(AccumulateLikes());
    }

    private IEnumerator AccumulateLikes()
    {
        int numLikes = Random.Range(3, 10);
        int curLikes = 1;

        for(int i = 0; i < numLikes; i++)
        {
            m_likeButtonText.text = "" + curLikes++;
            yield return new WaitForSeconds(Random.Range(0.3f, 1.0f));
        }
    }

    private IEnumerator LerpToTarget(GameObject target)
    {
        Vector3 origPosition = transform.position;
        float t = 0.0f;
        float lerpTime = 1.0f;
        while(t < lerpTime)
        {
            float dt = Time.deltaTime;

            t += dt;

            transform.position = Vector3.Lerp(origPosition, target.transform.position, t / lerpTime);

            yield return new WaitForEndOfFrame();
        }
    }
}
