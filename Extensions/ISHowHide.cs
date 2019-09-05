using UnityEngine;


    public interface IShowHide
    {
        void Show();
        void Hide();
    }
    public static class ShowHideExetensions
    {
        public static void Show(this Transform obj)
        {
            if (obj != null) Show(obj.gameObject);
        }

        public static void Hide(this GameObject obj)
        {
            if (obj == null) return;
            var showHide = obj.GetComponent<IShowHide>();
            if (showHide != null)
                showHide.Hide();
            else
                obj.SetActive(false);
        }

        public static void Hide(this Transform obj)
        {
            if (obj != null) Hide(obj.gameObject);
        }

        public static void Show(this GameObject obj)
        {
            if (obj == null) return;
            var showHide = obj.GetComponent<IShowHide>();
            if (showHide != null)
                showHide.Show();
            else
                obj.SetActive(true);

        }
        public static void Hide(this MonoBehaviour obj)
        {
            if (obj != null) Hide(obj.gameObject);
        }

        public static void Show(this MonoBehaviour obj)
        {
            if (obj == null) return;
            var showHide = obj.GetComponent<IShowHide>();
            if (showHide != null)
                showHide.Show();
            else
                obj.gameObject.SetActive(true);
        }
    }
