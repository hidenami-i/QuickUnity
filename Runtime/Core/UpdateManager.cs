using System;

namespace QuickUnity.Core
{
    public interface IUpdatable
    {
        void UpdateMe();
    }

    public interface IFixedUpdatable
    {
        void FixedUpdateMe();
    }

    public interface ILateUpdatable
    {
        void LateUpdateMe();
    }

    public class UpdateManager : SingletonMonoBehaviourBase<UpdateManager>
    {
        private IUpdatable[] updatables = new IUpdatable[16];
        private IFixedUpdatable[] fixedUpdatables = new IFixedUpdatable[16];
        private ILateUpdatable[] lateUpdatables = new ILateUpdatable[16];

        private int currentUpdatableLength;
        private int currentFixedUpdatableLength;
        private int currentLateUpdatableLength;

        protected override bool IsPersistent()
        {
            return false;
        }

        /// <summary>
        /// add the inheriting IUpdatable class.
        /// </summary>
        /// <param name="updatable"></param>
        public static void AddUpdatable(IUpdatable updatable)
        {
            if (Me == null)
            {
                return;
            }

            Me.InternalAddUpdatable(updatable);
        }

        /// <summary>
        /// remove the inheriting IUpdatable class.
        /// </summary>
        /// <param name="updatable"></param>
        public static void RemoveUpdatable(IUpdatable updatable)
        {
            if (Me == null)
            {
                return;
            }

            Me.InternalRemoveUpdatable(updatable);
        }

        /// <summary>
        /// add the inheriting IFixedUpdatable class.
        /// </summary>
        /// <param name="fixedUpdatable"></param>
        public static void AddFixedUpdatable(IFixedUpdatable fixedUpdatable)
        {
            if (Me == null)
            {
                return;
            }

            Me.InternalFixedAddUpdatable(fixedUpdatable);
        }

        /// <summary>
        /// remove the inheriting IFixedUpdatable class.
        /// </summary>
        /// <param name="fixedUpdatable"></param>
        public static void RemoveFixedUpdatable(IFixedUpdatable fixedUpdatable)
        {
            if (Me == null)
            {
                return;
            }

            Me.InternalFixedRemoveUpdatable(fixedUpdatable);
        }

        /// <summary>
        /// add the inheriting ILateUpdatable class.
        /// </summary>
        /// <param name="lateUpdatable"></param>
        public static void AddLateUpdatable(ILateUpdatable lateUpdatable)
        {
            if (Me == null)
            {
                return;
            }

            Me.InternalLateAddUpdatable(lateUpdatable);
        }

        /// <summary>
        /// remove the inheriting ILateUpdatable class.
        /// </summary>
        /// <param name="lateUpdatable"></param>
        public static void RemoveLateUpdatable(ILateUpdatable lateUpdatable)
        {
            if (Me == null)
            {
                return;
            }

            Me.InternalLateRemoveUpdatable(lateUpdatable);
        }

        private void InternalAddUpdatable(IUpdatable updatable)
        {
            if (updatable == null)
            {
                return;
            }

            if (updatables.Length == currentUpdatableLength)
            {
                Array.Resize(ref updatables, currentUpdatableLength * 2);
            }

            updatables[currentUpdatableLength++] = updatable;
        }

        private void InternalRemoveUpdatable(IUpdatable updatable)
        {
            for (var i = 0; i < updatables.Length; i++)
            {
                if (updatables[i] == null)
                {
                    continue;
                }

                if (updatables[i] == updatable)
                {
                    updatables[i] = null;
                    return;
                }
            }
        }

        private void InternalFixedAddUpdatable(IFixedUpdatable fixedUpdatable)
        {
            if (fixedUpdatable == null)
            {
                return;
            }

            if (fixedUpdatables.Length == currentFixedUpdatableLength)
            {
                Array.Resize(ref fixedUpdatables, currentFixedUpdatableLength * 2);
            }

            fixedUpdatables[currentFixedUpdatableLength++] = fixedUpdatable;
        }

        private void InternalFixedRemoveUpdatable(IFixedUpdatable fixedUpdatable)
        {
            for (var i = 0; i < fixedUpdatables.Length; i++)
            {
                if (fixedUpdatables[i] == null)
                {
                    continue;
                }

                if (fixedUpdatables[i] == fixedUpdatable)
                {
                    fixedUpdatables[i] = null;
                    return;
                }
            }
        }

        private void InternalLateAddUpdatable(ILateUpdatable lateUpdatable)
        {
            if (lateUpdatable == null)
            {
                return;
            }

            if (lateUpdatables.Length == currentLateUpdatableLength)
            {
                Array.Resize(ref lateUpdatables, currentLateUpdatableLength * 2);
            }

            lateUpdatables[currentLateUpdatableLength++] = lateUpdatable;
        }

        private void InternalLateRemoveUpdatable(ILateUpdatable lateUpdatable)
        {
            for (var i = 0; i < lateUpdatables.Length; i++)
            {
                if (lateUpdatables[i] == null)
                {
                    continue;
                }

                if (lateUpdatables[i] == lateUpdatable)
                {
                    lateUpdatables[i] = null;
                    return;
                }
            }
        }

        private void Update()
        {
            for (var i = 0; i < updatables.Length; i++)
            {
                if (updatables[i] == null)
                {
                    continue;
                }

                updatables[i].UpdateMe();
            }
        }

        private void FixedUpdate()
        {
            for (var i = 0; i < fixedUpdatables.Length; i++)
            {
                if (fixedUpdatables[i] == null)
                {
                    continue;
                }

                fixedUpdatables[i].FixedUpdateMe();
            }
        }

        private void LateUpdate()
        {
            for (var i = 0; i < lateUpdatables.Length; i++)
            {
                if (lateUpdatables[i] == null)
                {
                    continue;
                }

                lateUpdatables[i].LateUpdateMe();
            }
        }

        private void OnDisable()
        {
            for (var i = 0; i < updatables.Length; i++)
            {
                updatables[i] = null;
            }

            for (var i = 0; i < fixedUpdatables.Length; i++)
            {
                fixedUpdatables[i] = null;
            }

            for (var i = 0; i < lateUpdatables.Length; i++)
            {
                lateUpdatables[i] = null;
            }
        }
    }
}
