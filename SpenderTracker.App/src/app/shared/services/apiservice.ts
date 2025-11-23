import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { catchError, Observable, of, switchMap, tap } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class APIService {
    private readonly http = inject(HttpClient);
    private readonly baseUrl = "https://localhost:7010/api";
    private readonly cache: {[key: string]: object[]} = {};

    getById<T>(route: string, id: number): Observable<T> {
        return this.http.get<T>(`${this.baseUrl}/${route}/${id}`);
    }

    getAll<T>(route: string, useCache: boolean = false): Observable<T> {
        if (useCache) {
            if (route in this.cache) {
                return of(this.cache[route] as T);
            }
        }

        return this.http.get<T>(`${this.baseUrl}/${route}`).
            pipe(
                tap(data => {
                    this.cache[route] = (data as object[]);
                })
            );
    }

    insert<T>(route: string, entity: T): Observable<T> {
        return this.http.post<T>(`${this.baseUrl}/${route}`, entity);
    }

    update<T>(route: string, entity: T, id: number): Observable<T> {
        return this.http.put<T>(`${this.baseUrl}/${route}/${id}`, entity);
    }

    delete<T>(route: string, id: number): Observable<boolean> {
        return this.http.delete(`${this.baseUrl}/${route}/${id}`)
            .pipe(
                catchError(() => of(false)),
                switchMap(() => of(true))
            );
    }
}