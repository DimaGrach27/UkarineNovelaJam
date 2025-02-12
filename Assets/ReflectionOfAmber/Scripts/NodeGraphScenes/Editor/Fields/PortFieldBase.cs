using System;
using System.Collections.Generic;
using ReflectionOfAmber.Scripts.GameModelBlock;
using ReflectionOfAmber.Scripts.GameScene.ScreenPart;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace ReflectionOfAmber.Scripts.NodeGraphScenes.Editor.Fields
{
    public abstract class PortFieldBase
    {
        protected PortFieldBase(VisualElement container)
        {
            Container = container;
            LocalContainer = new VisualElement();
        }

        protected virtual int BlockSize { get; } = 60;
        protected virtual int ArrayElementSize { get; } = 40;
        protected virtual int TitleMargin { get; } = 4;
        protected virtual int ElementMargin { get; } = 12;
        protected virtual int BottomSpace { get; } = 18;

        private VisualElement Container { get;  }
        protected VisualElement LocalContainer { get;  }
        
        protected abstract string Title { get; }

        protected Port CachedPort;

        public virtual void Build(Port cachedPort)
        {
            CachedPort = cachedPort;
            
            var titleToggle = CreateToggleField(Title, enable =>
            {
                if (enable)
                {
                    OnEnable();
                    IncreaseSize(BlockSize);
                }
                else
                {
                    OnDisable();
                    ReduceSize(BlockSize);
                }
            }, false);

            titleToggle.style.marginLeft = TitleMargin;
            titleToggle.style.height = 20;
            
            LocalContainer.Add(titleToggle);
            // LocalContainer.style.borderBottomWidth = BottomSpace;

            Container.contentContainer.Add(LocalContainer);
        }
        
        protected virtual void OnEnable(){ }
        protected virtual void OnDisable(){ }

        protected Toggle CreateToggleField(string label, Action<bool> onChanged, bool initialValue)
        {
            var toggle = new Toggle(label)
            {
                value = initialValue
            };
            toggle.RegisterValueChangedCallback(evt => onChanged(evt.newValue));

            return toggle;
        }

        protected VisualElement CreateStatusesValueArrayField(
            string label, List<StatusesValue> array, 
            Action onArrayChanged)
        {
            var foldout = new Foldout
            {
                text = label,
                value = false
            };
            
            var elementsContainer = new VisualElement();
            elementsContainer.style.flexDirection = FlexDirection.Column;
            foldout.Add(elementsContainer);
            
            var addButton = new Button(() =>
            {
                array.Add(new StatusesValue());
                onArrayChanged?.Invoke();
                RefreshArrayFields(array, elementsContainer, onArrayChanged);
                
                IncreaseSize(ArrayElementSize);
            })
            {
                text = "Add"
            };
            foldout.Add(addButton);
            
            RefreshArrayFields(array, elementsContainer, onArrayChanged);

            return foldout;
        }

        protected void ReduceSize(float value)
        {
            float portHeight = CachedPort.resolvedStyle.height;

            portHeight -= value;
            CachedPort.style.height = portHeight;
        }
        
        protected void IncreaseSize(float value)
        {
            float portHeight = CachedPort.resolvedStyle.height;

            portHeight += value;
            CachedPort.style.height = portHeight;
        }

        private void RefreshArrayFields(List<StatusesValue> array, VisualElement container, Action onArrayChanged)
        {
            container.Clear();

            for (int i = 0; i < array.Count; i++)
            {
                int index = i;

                var mainContainer = new VisualElement()
                {
                    style =
                    {
                        flexDirection = FlexDirection.Row
                    }
                };
                
                var elementContainer = new VisualElement
                {
                    style =
                    {
                        flexDirection = FlexDirection.Column,
                        alignItems = Align.FlexStart,
                        height = ArrayElementSize,
                    }
                };

                var toggleField = new Toggle("Value")
                {
                    value = array[i].value,
                };
                
                var toggleLabel = toggleField.Q<Label>();
                toggleLabel.style.marginRight = -92;
                
                toggleField.RegisterValueChangedCallback(evt =>
                {
                    array[index].value = evt.newValue;
                    onArrayChanged?.Invoke();
                });
                elementContainer.Add(toggleField);
                
                var enumField = new EnumField("Status", array[i].status);
                // var enumLabel = enumField.Q<Label>();
                // enumLabel.style.marginRight = -92;
                
                enumField.Init(StatusEnum.NONE);
                enumField.RegisterValueChangedCallback(evt =>
                {
                    array[index].status = (StatusEnum)evt.newValue;
                    onArrayChanged?.Invoke();
                });
                elementContainer.Add(enumField);
                
                var removeButton = new Button(() =>
                {
                    array.RemoveAt(index);
                    onArrayChanged?.Invoke();
                    RefreshArrayFields(array, container, onArrayChanged);

                    ReduceSize(ArrayElementSize);
                })
                {
                    text = "X"
                };
                
                mainContainer.Add(elementContainer);
                mainContainer.Add(removeButton);
                
                container.Add(mainContainer);
            }

            container.style.height = ArrayElementSize * array.Count;
        }

        protected VisualElement CreateArrayField<T>(string label, List<T> array, Action onArrayChanged, Func<T> createNewItem)
        {
            var foldout = new Foldout
            {
                text = label,
                value = false
            };
            
            var elementsContainer = new VisualElement();
            elementsContainer.style.flexDirection = FlexDirection.Column;
            foldout.Add(elementsContainer);
            
            var addButton = new Button(() =>
            {
                array.Add(createNewItem());
                onArrayChanged?.Invoke();
                RefreshArrayFields(array, elementsContainer, onArrayChanged);
            })
            {
                text = "Add"
            };
            foldout.Add(addButton);

            // Оновлення полів
            RefreshArrayFields(array, elementsContainer, onArrayChanged);

            return foldout;
        }

        private void RefreshArrayFields<T>(List<T> array, VisualElement container, Action onArrayChanged)
        {
            container.Clear();
            
            for (int i = 0; i < array.Count; i++)
            {
                int index = i;

                // Контейнер для елемента
                var elementContainer = new VisualElement
                {
                    style =
                    {
                        flexDirection = FlexDirection.Row,
                        alignItems = Align.Center
                    }
                };

                // Поле редагування (приклад для string, можна адаптувати під інші типи)
                var textField = new TextField($"Element {i}")
                {
                    value = array[i].ToString()
                };
                
                textField.RegisterValueChangedCallback(evt =>
                {
                    if (typeof(T) == typeof(string))
                    {
                        array[index] = (T)(object)evt.newValue;
                        onArrayChanged?.Invoke();
                    }
                });
                elementContainer.Add(textField);

                // Кнопка видалення
                var removeButton = new Button(() =>
                {
                    array.RemoveAt(index);
                    onArrayChanged?.Invoke();
                    RefreshArrayFields(array, container, onArrayChanged);
                })
                {
                    text = "X"
                };
                elementContainer.Add(removeButton);

                // Додаємо елемент до контейнера
                container.Add(elementContainer);
            }
        }
    }
}