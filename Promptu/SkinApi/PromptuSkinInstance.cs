// Copyright 2022 Zach Johnson
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace ZachJohnson.Promptu.SkinApi
{
    using ZachJohnson.Promptu.PluginModel;
    using ZachJohnson.Promptu.Skins;

    public class PromptuSkinInstance
    {
        private ILayoutManager layoutManager;
        private IPrompt prompt;
        private ISuggestionProvider suggestionProvider;
        private PropertiesAndOptions informationBoxPropertiesAndOptions;

        public PromptuSkinInstance(
            ILayoutManager layoutManager, 
            IPrompt prompt, 
            ISuggestionProvider suggestionProvider,
            PropertiesAndOptions informationBoxPropertiesAndOptions)
            : this(
                layoutManager, 
                prompt, 
                suggestionProvider,
                informationBoxPropertiesAndOptions,
                false)
        {
        }

        internal PromptuSkinInstance(
            ILayoutManager layoutManager,
            IPrompt prompt,
            ISuggestionProvider suggestionProvider,
            PropertiesAndOptions informationBoxPropertiesAndOptions,
            bool isDefault)
        {
            this.informationBoxPropertiesAndOptions =
                informationBoxPropertiesAndOptions ??
                InternalGlobals.GuiManager.ToolkitHost.Factory.ConstructDefaultInformationBoxPropertiesAndOptions();

            if (!isDefault)
            {
                PromptuSkinInstance defaultSkin = InternalGlobals.GuiManager.ToolkitHost.CreateDefaultSkinInstance();
                this.layoutManager = layoutManager ?? defaultSkin.LayoutManager;
                this.prompt = prompt ?? defaultSkin.Prompt;
                this.suggestionProvider = suggestionProvider ?? defaultSkin.SuggestionProvider;
            }
            else
            {
                this.layoutManager = layoutManager;
                this.prompt = prompt;
                this.suggestionProvider = suggestionProvider;
            }
        }

        public PropertiesAndOptions InformationBoxPropertiesAndOptions
        {
            get { return this.informationBoxPropertiesAndOptions; }
        }

        public ILayoutManager LayoutManager
        {
            get { return this.layoutManager; }
        }

        public IPrompt Prompt
        {
            get { return this.prompt; }
        }

        public ISuggestionProvider SuggestionProvider
        {
            get { return this.suggestionProvider; }
        }

        public ITextInfoBox CreateTextInfoBox()
        {
            ITextInfoBox infoBox = this.CreateTextInfoBoxCore();
            if (infoBox == null)
            {
                infoBox = InternalGlobals.GuiManager.ToolkitHost.Factory.ConstructDefaultTextInfoBox();
            }

            PropertiesAndOptions propertiesAndOptions = this.InformationBoxPropertiesAndOptions;
            propertiesAndOptions.ApplyTo(infoBox);
            return infoBox;
        }

        public IProgressInfoBox CreateProgressInfoBox()
        {
            IProgressInfoBox infoBox = this.CreateProgressInfoBoxCore();
            if (infoBox == null)
            {
                infoBox = InternalGlobals.GuiManager.ToolkitHost.Factory.ConstructDefaultProgressInfoBox();
            }

            PropertiesAndOptions propertiesAndOptions = this.InformationBoxPropertiesAndOptions;
            propertiesAndOptions.ApplyTo(infoBox);
            return infoBox;
        }

        public void GiveContextualErrorMessage(string message)
        {
            PromptHandler.GetInstance().GiveContextualError(message);
        }

        protected virtual ITextInfoBox CreateTextInfoBoxCore()
        {
            return null;
        }

        protected virtual IProgressInfoBox CreateProgressInfoBoxCore()
        {
            return null;
        }
    }
}
