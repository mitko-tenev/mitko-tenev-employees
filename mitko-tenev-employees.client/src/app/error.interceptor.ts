import { Injectable } from '@angular/core';
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError((error: HttpErrorResponse) => {
        let errorMsg = 'An unknown error occurred!';

        if (error.error && error.error.error) {
          // Our API sent a custom error
          errorMsg = error.error.error;
        } else if (error.message) {
          // Some other error
          errorMsg = error.message;
        }

        return throwError(() => new Error(errorMsg));
      })
    );
  }
}
