/*---------------------------------------------------------------------------------------------
 *  (c) 2024 TOYOYA MOTOR CORPORATION, TOYOTA CENTRAL R&D LABS., INC.
 *  Licensed under the MIT License. See License.txt in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
 
using System.IO;

namespace VRMD.IO {
    public class FileReader {
        private TextReader Reader;

        public FileReader (string path) {

#if UNITY_EDITOR
            if(!System.IO.File.Exists(path))
            {
                throw new System.Exception("file not found;"+path);
            }
            this.Reader = new StreamReader (path);
#elif UNITY_IOS
            if(!System.IO.File.Exists(path))
            {
                throw new System.Exception("file not found;"+path);
            }
            this.Reader = new StreamReader (path);
#elif UNITY_ANDROID
            UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get(path);
            www.SendWebRequest();
            while (!www.isDone)
            {
            }
            this.Reader = new StringReader(www.downloadHandler.text);
#else

#endif
        }

        public static string GetUnityStreamingAssetPath (string path) {
#if UNITY_EDITOR
            return UnityEngine.Application.streamingAssetsPath + "/" + path;
#elif UNITY_IOS
            return UnityEngine.Application.streamingAssetsPath + "/" + path;
#elif UNITY_ANDROID
            return "jar:file://" + UnityEngine.Application.dataPath + "/!/assets" + "/" + path;
#else

#endif
        }

        public string ReadLine () => this.Reader.ReadLine ();
    }
}