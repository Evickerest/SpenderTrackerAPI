import { Component, computed, inject, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-error',
  imports: [],
  templateUrl: './error.html',
  styleUrl: './error.css',
})
export class ErrorPage {
    private route = inject(ActivatedRoute);
    error = computed(() => this.route.snapshot.queryParamMap.get("error"));
}
