namespace GameReplay
{
    public struct Replay {
        public bool isValid { get { return (null != replayURL && 0 != replayURL.Length); } }

        public void clear() {
            new System.IO.FileInfo(replayURL).Delete();
        }

        internal Replay(string inReplayURL) {
            replayURL = inReplayURL;
        }

        internal string replayURL;
    }
}
