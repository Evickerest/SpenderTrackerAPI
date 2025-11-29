import { ApplicationConfig, inject, provideBrowserGlobalErrorListeners, provideZonelessChangeDetection } from '@angular/core';
import { NavigationError, provideRouter, Router, withComponentInputBinding, withNavigationErrorHandler } from '@angular/router';

import { routes } from './app.routes';
import { provideHttpClient } from '@angular/common/http';

function handleError(error: NavigationError) {
    const router = inject(Router);
    console.error(`Navigation error occurred: ${error.error.error}`);
    router.navigate(['/error'], { queryParams: {error: error.error.error}});
}

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZonelessChangeDetection(),
    provideHttpClient(),
    provideRouter(routes, withComponentInputBinding(), withNavigationErrorHandler(handleError))
  ]
};
