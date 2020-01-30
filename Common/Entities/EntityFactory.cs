using System;


namespace Common.Entities
{

    public abstract class EntityFactory<TModel, TEntity>    where TModel    : class 
                                                            where TEntity   : class 
    {

        private readonly TModel     _model;
        private readonly TEntity    _original;


        protected EntityFactory( TModel model, TEntity original ) {

            if ( Object.ReferenceEquals( model, null ) ) {
                throw new ArgumentNullException( "model" );
            }

            _model      = model;
            _original   = original;

        }


        protected TModel Model {
            get { return _model; }
        }

        protected TEntity Original {
            get { return _original; }
        }


        public abstract TEntity Create();

    }

}
