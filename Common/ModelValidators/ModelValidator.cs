using Common.Logging.Simple;
using System;


namespace Common.ModelValidators
{

    public abstract class ModelValidator {

        private readonly SimpleLog      _log;


        protected ModelValidator() {

            _log = new SimpleLog();

        }


        public SimpleLog Log {
            get { return _log; }
        }



        public abstract void Validate();



        public override String ToString() {

            return String.Format( "Count = {0}", Log.Count );

        }



        protected SimpleLogItem CreateInfo( String message, String description = null, Exception exception = null ) {

            return CreateItem( SimpleLogItemSeverity.Info, message, description, exception );

        }


        protected SimpleLogItem CreateWarn( String message, String description = null, Exception exception = null ) {

            return CreateItem( SimpleLogItemSeverity.Warn, message, description, exception );

        }


        protected SimpleLogItem CreateError( String message, String description = null, Exception exception = null ) {

            return CreateItem( SimpleLogItemSeverity.Error, message, description, exception );

        }


        protected SimpleLogItem CreateItem( SimpleLogItemSeverity severity, String message, String description = null, Exception exception = null ) {

            return new SimpleLogItem( severity, message, description, exception );

        }




        protected SimpleLogItem LogInfo( String message, String description = null, Exception exception = null ) {

            return LogItem( SimpleLogItemSeverity.Info, message, description, exception );

        }


        protected SimpleLogItem LogWarn( String message, String description = null, Exception exception = null ) {

            return LogItem( SimpleLogItemSeverity.Warn, message, description, exception );

        }


        protected SimpleLogItem LogError( String message, String description = null, Exception exception = null ) {

            return LogItem( SimpleLogItemSeverity.Error, message, description, exception );

        }


        protected SimpleLogItem LogItem( SimpleLogItemSeverity severity, String message, String description = null, Exception exception = null ) {

            SimpleLogItem item = CreateItem( severity, message, description, exception );

            Log.Add( item );

            return item;

        }

    }




    public abstract class ModelValidator<TModel> : ModelValidator where TModel : class {

        private readonly TModel     _model;


        protected ModelValidator( TModel model ) {

            if ( Object.ReferenceEquals( model, null ) ) {
                throw new ArgumentNullException( "model" );
            }


            _model      = model;

        }


        protected TModel Model {
            get { return _model; }
        }

    }

}
