// <copyright file="App.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace imdbdrinks_ratingsmodule
{
    using System;
    using imdbdrinks_ratingsmodule.Repositories;
    using imdbdrinks_ratingsmodule.Services;
    using imdbdrinks_ratingsmodule.ViewModels;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.UI.Xaml;

    /// <summary>
    /// Represents the main application class for the IMDb Drinks Ratings Module.
    /// </summary>
    public partial class App : Application
    {
        private Window? window;

        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class.
        /// </summary>
        public App()
        {
            this.InitializeComponent();

            // Configure services and DI container
            this.Services = this.ConfigureServices();
        }

        /// <summary>
        /// Gets the service provider for dependency injection.
        /// </summary>
        public IServiceProvider Services { get; }

        /// <summary>
        /// Handles the application launch event.
        /// </summary>
        /// <param name="args">Arguments for launch event.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            // Use DI to get MainWindow with dependencies injected
            this.window = this.Services.GetRequiredService<MainWindow>();
            this.window.Activate();
        }

        private IServiceProvider ConfigureServices()
        {
            // Load appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            var services = new ServiceCollection();

            // Add configuration to DI container
            services.AddSingleton<IConfiguration>(configuration);
            services.AddSingleton<DatabaseConnection>();

            // Add repositories, services, and view models to DI container
            services.AddSingleton<IReviewRepository, DatabaseReviewRepository>();
            services.AddSingleton<IRatingRepository, DatabaseRatingRepository>();

            services.AddSingleton<RatingService>();
            services.AddSingleton<ReviewService>();

            services.AddSingleton<RatingViewModel>();
            services.AddSingleton<ReviewViewModel>();
            services.AddSingleton<MainViewModel>();

            services.AddTransient<IRatingService,RatingService>();
            services.AddTransient<IReviewService,ReviewService>();

            // Register MainWindow
            services.AddSingleton<MainWindow>();

            return services.BuildServiceProvider();
        }
    }
}
