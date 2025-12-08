using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class UIOptions : MonoBehaviour
{
    private int volume = 10;
    private int sfx = 10;
    public TextMeshProUGUI volumeLabel;
    public TextMeshProUGUI sfxLabel;
    public void Start()
    {
        volumeLabel.text = volume.ToString();
        sfxLabel.text = sfx.ToString();
    }
    public void AddVolume()
    {
        volume++;
        volume = Mathf.Clamp(volume, 0, 10);
        volumeLabel.text = volume.ToString();
        AudioManager.Instance.MusicSource.volume = volume /10f;
    }
    public void MinusVolume()
    {
        volume--;
        volume = Mathf.Clamp(volume, 0, 10);
        volumeLabel.text = volume.ToString();
        AudioManager.Instance.MusicSource.volume = volume/10f;
    }
    public void AddSFX()
    {
        sfx++;
        sfx = Mathf.Clamp(sfx, 0, 10);
        sfxLabel.text = sfx.ToString();
         AudioManager.Instance.SfxSource.volume = sfx/10f;
    }
    public void MinusSFX()
    {
        sfx--;
        sfx = Mathf.Clamp(sfx, 0, 10);
        sfxLabel.text = sfx.ToString();
        AudioManager.Instance.SfxSource.volume = sfx/10f;
    }

}
