import { Component, computed, inject, input, model } from '@angular/core';
import { FormValueControl } from '@angular/forms/signals';
import { rxResource } from '@angular/core/rxjs-interop';
import { APIService } from '../../services/apiservice';

@Component({
  selector: 'app-select-box',
  imports: [],
  templateUrl: './select-box.html',
  styleUrl: './select-box.css',
})
export class SelectBox implements FormValueControl<number> {
    private readonly apiService = inject(APIService);

    value = model<number>(NaN);
    resourceName = input.required<string>();
    resourceRoute = input.required<string>({ alias: "route"});
    displayMember = input.required<string>();
    valueMember = input.required<string>();
    useCache = input<boolean>(true);
    resource = rxResource({
        stream: () => this.apiService.getAll<{[key: string]: any}[]>(this.resourceRoute(), this.useCache())
    });

    data = computed(() => this.resource.hasValue() ? this.resource.value() : []);
    isLoading = computed(() => this.resource.isLoading());
    error = computed(() => this.resource.error());
}
