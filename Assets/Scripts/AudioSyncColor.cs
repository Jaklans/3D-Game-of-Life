using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSyncColor : AudioSyncer
{
    public Color[] beatColors;
    public Color restColor;
    public ParticleSystem pSystem;

    private int m_randomIndx;
    private GameObject m_img;

    private void Start()
    {
        m_img = this.gameObject;
        var main = pSystem.main;
        main.startColor = m_img.GetComponent<MeshRenderer>().material.color;
    }

    private IEnumerator MoveToColor(Color _target)
    {
        Color _curr = m_img.GetComponent<MeshRenderer>().material.color;
        Color _init = _curr;

        float _timer = 0;

        while (_curr != _target)
        {
            _curr = Color.Lerp(_init, _target, _timer / timeToBeat);
            _timer += Time.deltaTime;

            m_img.GetComponent<MeshRenderer>().material.color = _curr;

            yield return null;
        }
        

        m_isBeat = false;
    }

    private Color RandomColor()
    {
        if (beatColors == null || beatColors.Length == 0)
        {
            return Color.white;
        }

        m_randomIndx = Random.Range(0, beatColors.Length);

        return beatColors[m_randomIndx];
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (m_isBeat)
        {
            return;
        }

        Color holder = Color.Lerp(
            m_img.GetComponent<MeshRenderer>().material.color, restColor, restSmoothTime * Time.deltaTime);

        m_img.GetComponent<MeshRenderer>().material.SetColor("_Color",(new Color(holder.r, holder.g, holder.b, 1.0f)));

        var main = pSystem.main;
        main.startColor = m_img.GetComponent<MeshRenderer>().material.color;
    }

    public override void OnBeat()
    {
        base.OnBeat();

        Color _c = RandomColor();

        pSystem.Play();

        StopCoroutine("MoveToColor");
        StartCoroutine("MoveToColor", _c);
    }
}
