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
        private bool isMuted;
        private float previousMusicVolume;
        private float previousSFXVolume;


        public Audio()
        {
            _soundInstances = new Dictionary<string, SoundEffectInstance>();
            _songs = new Dictionary<string, Song>();
            _SFXVolume = 0.2f;
            isMuted = false; // เริ่มต้นเสียงเปิด
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
        
        // ฟังก์ชันสำหรับตั้งค่าระดับเสียงทั้งหมด
        public void SetVolume(float volume)
        {
            // ตั้งระดับเสียงของเพลง
            MediaPlayer.Volume = MathHelper.Clamp(volume, 0f, 1f);

            // ตั้งระดับเสียงของ SFX
            _SFXVolume = MathHelper.Clamp(volume, 0f, 1f);

            // ตั้งระดับเสียงของทุกๆ Instance ของ SFX
            foreach (var _sinst in _soundInstances.Values)
            {
                _sinst.Volume = _SFXVolume;
            }
        }

        // ฟังก์ชันเพิ่มระดับเสียง
        public void IncreaseVolume(float increment = 0.1f)
        {
            if (!isMuted)  // ตรวจสอบว่าถึงแม้จะไม่ปิดเสียง (ไม่ใช่ mute) ก็สามารถเพิ่มเสียงได้
            {
                float newVolume = MathHelper.Clamp(MediaPlayer.Volume + increment, 0f, 1f);
                SetVolume(newVolume);  // ปรับระดับเสียงของเพลงและ SFX

                // ถ้าเสียงถูกเพิ่มแล้ว, เปลี่ยนสถานะเป็นเปิดเสียง (Unmute)
                if (isMuted)
                {
                    UnmuteAll();
                }
            }
        }

        // ฟังก์ชันลดระดับเสียง
        public void DecreaseVolume(float decrement = 0.1f)
        {
            if (!isMuted)  // ตรวจสอบว่าถึงแม้จะไม่ปิดเสียง (ไม่ใช่ mute) ก็สามารถลดเสียงได้
            {
                float newVolume = MathHelper.Clamp(MediaPlayer.Volume - decrement, 0f, 1f);
                SetVolume(newVolume);  // ปรับระดับเสียงของเพลงและ SFX

                // ถ้าลดเสียงจนถึง 0, เปลี่ยนสถานะเป็นปิดเสียง (Mute)
                if (newVolume <= 0f)
                {
                    MuteAll();
                }
            }
        }

        // ฟังก์ชันปิดเสียงทั้งหมด
        public void MuteAll()
        {
            if (!isMuted)  // ถ้ายังไม่ได้ปิดเสียง
            {
                // เก็บระดับเสียงก่อนที่จะทำการปิดเสียง
                previousMusicVolume = MediaPlayer.Volume;
                previousSFXVolume = _SFXVolume;

                // ปิดเสียงเพลงทั้งหมด
                MediaPlayer.Volume = 0f;

                // ปิดเสียงเอฟเฟกต์ทั้งหมด
                foreach (var _sinst in _soundInstances.Values)
                {
                    _sinst.Volume = 0f;
                }

                isMuted = true;  // เปลี่ยนสถานะเป็นเสียงปิด
            }
        }

        // ฟังก์ชันเปิดเสียงทั้งหมด
        public void UnmuteAll()
        {
            if (isMuted)  // ถ้ามีการปิดเสียงแล้ว
            {
                // คืนค่าเสียงที่เคยเก็บไว้
                MediaPlayer.Volume = previousMusicVolume;
                foreach (var _sinst in _soundInstances.Values)
                {
                    _sinst.Volume = previousSFXVolume;
                }

                isMuted = false;  // เปลี่ยนสถานะเป็นเสียงเปิด
            }
        }

        // ฟังก์ชันตรวจสอบสถานะเสียง
        public bool IsMuted()
        {
            return isMuted;
        }

        // ฟังก์ชันดึงระดับเสียงปัจจุบัน
        public float GetCurrentVolume()
        {
            return MediaPlayer.Volume; // คืนค่าระดับเสียงของเพลง
        }
    }
}
