using System.Reflection;
using System.Text.RegularExpressions;
using NUnit.Framework;
using ReflectionOfAmber.Scripts.Input;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.EditMode
{
    public class InputServiceTests
    {
        private MethodInfo _setAction;

        [SetUp]
        public void Setup()
        {
            _setAction = typeof(InputService).GetMethod("SetAction", BindingFlags.Instance | BindingFlags.NonPublic);
        }

        private void InvokeSetAction(InputService service, InputAction action)
        {
            _setAction.Invoke(service, new object[] { action });
        }

        private class MockListener : IInputListener
        {
            public InputAction LastAction = InputAction.NONE;
            public void OnInputAction(InputAction inputAction)
            {
                LastAction = inputAction;
            }
        }

        [Test]
        public void ForceRedirectInputAndRemoveForceRedirected_WorksAndLogsError()
        {
            var service = new InputService();
            var listener = new MockListener();
            var wrongListener = new MockListener();

            service.AddListener(listener);
            service.ForceRedirectInput(listener);

            InvokeSetAction(service, InputAction.SPACE);
            Assert.AreEqual(InputAction.SPACE, listener.LastAction);

            LogAssert.Expect(LogType.Error, new Regex("Wrong redirected listener"));
            service.RemoveForceRedirected(wrongListener);

            InvokeSetAction(service, InputAction.PAUSE);
            Assert.AreEqual(InputAction.PAUSE, listener.LastAction);

            service.RemoveForceRedirected(listener);

            InvokeSetAction(service, InputAction.SPACE);
            Assert.AreEqual(InputAction.SPACE, listener.LastAction);
        }
    }
}
