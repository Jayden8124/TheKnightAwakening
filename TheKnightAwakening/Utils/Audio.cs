using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;

namespace TheKnightAwakening
{
    public class Audio
    {
        private Dictionary<string, SoundEffectInstance> _soundInstances;
        public Dictionary<string, Song> _songs;
        private float _SFXVolume;

        public Audio()
        {
            _soundInstances = new Dictionary<string, SoundEffectInstance>();
            _songs = new Dictionary<string, Song>();
            _SFXVolume = 0.3f;
        }

        public void LoadSounds(ContentManager Content)
        {
            // Song Loading
            _songs["Bgm"] = Content.Load<Song>("Bgm");
            _songs["Boss_Bgm"] = Content.Load<Song>("Boss_Bgm");

            // Sound Effect Loading
            SoundEffect _sound1 = Content.Load<SoundEffect>("Slime_Die");
            SoundEffect _sound2 = Content.Load<SoundEffect>("Skeleton_Dead");
            SoundEffect _sound3 = Content.Load<SoundEffect>("Sword_SFX");
            SoundEffect _sound4 = Content.Load<SoundEffect>("Death_SFX");
            SoundEffect _sound5 = Content.Load<SoundEffect>("Medusa_Dead");
            SoundEffect _sound6 = Content.Load<SoundEffect>("Medusa_Scream");
            SoundEffect _sound7 = Content.Load<SoundEffect>("Save_SFX");
            SoundEffect _sound8 = Content.Load<SoundEffect>("Victory_SFX");
            SoundEffect _sound9 = Content.Load<SoundEffect>("Coin_Collect");
            SoundEffect _sound10 = Content.Load<SoundEffect>("Item_Obtain");

            _soundInstances.Add("Slime_Die", _sound1.CreateInstance());
            _soundInstances.Add("Skeleton_Dead", _sound2.CreateInstance());
            _soundInstances.Add("Sword_SFX", _sound3.CreateInstance());
            _soundInstances.Add("Death_SFX", _sound4.CreateInstance());
            _soundInstances.Add("Medusa_Dead", _sound5.CreateInstance());
            _soundInstances.Add("Medusa_Scream", _sound6.CreateInstance());
            _soundInstances.Add("Save_SFX", _sound7.CreateInstance());
            _soundInstances.Add("Victory_SFX", _sound8.CreateInstance());
            _soundInstances.Add("Coin_Collect", _sound9.CreateInstance());
            _soundInstances.Add("Item_Obtain", _sound10.CreateInstance());

            foreach (var _sinst in _soundInstances.Values)
            {
                // Setup SFX Volume for each Instances
                _sinst.Volume = _SFXVolume;
            }
        }

        public void PlayEffect(string name)
        {
            if (_soundInstances.ContainsKey(name))
            {
                _soundInstances[name].Play();
            }
        }

        public void PlayMusic(string name, float volume)
        {
            if (_songs.ContainsKey(name))
            {
                MediaPlayer.Stop();
                MediaPlayer.Volume = MathHelper.Clamp(volume, 0f, 1f);
                MediaPlayer.Play(_songs[name]);
                MediaPlayer.IsRepeating = true;
            }
        }

        public void StopAllEffects()
        {
            foreach (var _sinst in _soundInstances.Values)
            {
                _sinst.Stop();
            }
        }

        public void SetMusicVolume(float volume)
        {
            MediaPlayer.Volume = MathHelper.Clamp(volume, 0f, 1f);
        }

        public void SetSFXVolume(float volume)
        {
            _SFXVolume = MathHelper.Clamp(volume, 0f, 1f);
            foreach (var _sinst in _soundInstances.Values)
            {
                _sinst.Volume = _SFXVolume;
            }
        }
    }
}
