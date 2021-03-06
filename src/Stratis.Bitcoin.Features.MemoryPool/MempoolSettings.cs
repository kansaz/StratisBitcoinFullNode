﻿using System;
using Stratis.Bitcoin.Configuration;

namespace Stratis.Bitcoin.Features.MemoryPool
{
    /// <summary>
    /// Configuration of mempool features and limits.
    /// </summary>
    public class MempoolSettings
    {
        /// <summary>Default value for "blocksonly" option.</summary>
        /// <seealso cref="RelayTxes"/>
        private const bool DefaultBlocksOnly = false;

        // Default value for "whitelistrelay" option.
        /// <seealso cref="WhiteListRelay"/>
        private const bool DefaultWhiteListRelay = true;

        /// <summary>Maximal size of the transaction memory pool in megabytes.</summary>
        public int MaxMempool { get; set; }

        /// <summary>Maximum number of hours to keep transactions in the mempool.</summary>
        public int MempoolExpiry { get; set; }

        /// <summary><c>true</c> to require high priority for relaying free or low-fee transactions.</summary>
        public bool RelayPriority { get; set; }

        /// <summary>Number of kB/minute at which free transactions (with enough priority) will be accepted.</summary>
        public int LimitFreeRelay { get; set; }

        /// <summary>Maximum number of ancestors of a transaction in mempool (including itself).</summary>
        public int LimitAncestors { get; set; }

        /// <summary>Maximal size in kB of ancestors of a transaction in mempool (including itself).</summary>
        public int LimitAncestorSize { get; set; }

        /// <summary>Maximum number of descendants any ancestor can have in mempool (including itself).</summary>
        public int LimitDescendants { get; set; }

        /// <summary>Maximum size in kB of descendants any ancestor can have in mempool (including itself).</summary>
        public int LimitDescendantSize { get; set; }

        /// <summary><c>true</c> to enable transaction replacement in the memory pool.</summary>
        public bool EnableReplacement { get; set; }

        /// <summary>Maximum number of orphan transactions kept in memory.</summary>
        public int MaxOrphanTx { get; set; }

        /// <summary><c>true</c> to enable bandwidth saving setting to send and received confirmed blocks only.</summary>
        public bool RelayTxes { get; set; }

        /// <summary><c>true</c> to accept relayed transactions received from whitelisted peers even when not relaying transactions.</summary>
        public bool WhiteListRelay { get; set; }

        public NodeSettings NodeSettings { get; private set; }

        /// <summary>Records a script used for overriding loaded settings</summary>
        private readonly Action<MempoolSettings> callback;

        /// <summary>
        /// Constructor for the mempool settings
        /// </summary>
        /// <param name="callback">Script applied during load to override configured settings</param>
        public MempoolSettings(Action<MempoolSettings> callback)
        {
            this.callback = callback;
        }

        /// <summary>
        /// Constructor for the mempool settings.
        /// </summary>
        /// <param name="nodeSettings">The node's configuration settings.</param>
        /// <param name="callback">Script applied during load to override configured settings</param>
        public MempoolSettings(NodeSettings nodeSettings, Action<MempoolSettings> callback = null)
            :this(callback)
        {
            this.Load(nodeSettings);
        }

        /// <summary>
        /// Loads the mempool settings from the application configuration.
        /// </summary>
        /// <param name="nodeSettings">Node configuration.</param>
        public void Load(NodeSettings nodeSettings)
        {
            var config = nodeSettings.ConfigReader;
            this.MaxMempool = config.GetOrDefault("maxmempool", MempoolValidator.DefaultMaxMempoolSize);
            this.MempoolExpiry = config.GetOrDefault("mempoolexpiry", MempoolValidator.DefaultMempoolExpiry);
            this.RelayPriority = config.GetOrDefault("relaypriority", MempoolValidator.DefaultRelaypriority);
            this.LimitFreeRelay = config.GetOrDefault("limitfreerelay", MempoolValidator.DefaultLimitfreerelay);
            this.LimitAncestors = config.GetOrDefault("limitancestorcount", MempoolValidator.DefaultAncestorLimit);
            this.LimitAncestorSize = config.GetOrDefault("limitancestorsize", MempoolValidator.DefaultAncestorSizeLimit);
            this.LimitDescendants = config.GetOrDefault("limitdescendantcount", MempoolValidator.DefaultDescendantLimit);
            this.LimitDescendantSize = config.GetOrDefault("limitdescendantsize", MempoolValidator.DefaultDescendantSizeLimit);
            this.EnableReplacement = config.GetOrDefault("mempoolreplacement", MempoolValidator.DefaultEnableReplacement);
            this.MaxOrphanTx = config.GetOrDefault("maxorphantx", MempoolOrphans.DefaultMaxOrphanTransactions);
            this.RelayTxes = !config.GetOrDefault("blocksonly", DefaultBlocksOnly);
            this.WhiteListRelay = config.GetOrDefault("whitelistrelay", DefaultWhiteListRelay);

            this.NodeSettings = nodeSettings;

            this.callback?.Invoke(this);
        }
    }
}