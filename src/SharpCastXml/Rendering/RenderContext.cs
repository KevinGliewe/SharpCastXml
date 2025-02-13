using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Reflection;
using Autofac;
using SharpCastXml.CppModel;
using SharpCastXml.Rendering.Extensions;


namespace SharpCastXml.Rendering
{

    public class RenderingContext
    {
        public record RenderingViewRegistration(
            Type ModelType,
            Type AnnotationType,
            Type ViewType,
            string ViewName
        );

        private IContainer _container;

        private List<RenderingViewRegistration> _viewRegistrations = new();

        public IContainer Container => _container;

        public RenderingContext(IContainer container)
        {
            _container = container;
        }

        public void RegisterView<TView, TModel, TAnnotation>(string annotationTypeName = null)
            where TModel : CppElement
            where TView : CppElementView<TModel, TAnnotation>
        {
            _viewRegistrations.Add(new RenderingViewRegistration(
                typeof(TModel),
                typeof(TAnnotation),
                typeof(TView),
                annotationTypeName
            ));
        }

        public void RegisterView<TView, TModel, TAnnotation>()
            where TModel : CppElement
            where TView : CppElementView<TModel, TAnnotation>
        {
            var viewName = typeof(TView).Name;

            // Check for a ModelTypeNameAttribute
            var modelTypeNameAttribute = typeof(TView).GetCustomAttribute<ViewNameAttribute>();
            if (modelTypeNameAttribute != null)
            {
                viewName = modelTypeNameAttribute.Name;
            }

            _viewRegistrations.Add(new RenderingViewRegistration(
                typeof(TModel),
                typeof(TAnnotation),
                typeof(TView),
                viewName
            ));
        }

        public IEnumerable<RenderingViewRegistration> GetRenderingViewRegistrations(CppElement model)
        {
            string viewName = model.GetViewName();
            Type modelType = model.GetType();

            return _viewRegistrations.FindAll(registration =>
            {
                if(registration.ModelType != modelType)
                    return false;

                if (registration.ViewName == null)
                    return true;

                if (registration.ViewName == viewName)
                    return true;

                return false;
            });
        }

        public void Render(CppElement model, IndentedTextWriter writer)
        {
            foreach(var registration in GetRenderingViewRegistrations(model))
            {
                Type viewType = registration.ViewType;
                var view = _container.Resolve(viewType);

                IRenderable renderable = view as IRenderable;

                renderable.Init(model, writer, this);
                renderable.Render();
            }
        }
    }
}