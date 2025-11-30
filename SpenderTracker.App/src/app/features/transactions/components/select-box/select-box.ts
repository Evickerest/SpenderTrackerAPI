import { Component, computed, inject, input, model} from '@angular/core';
import { APIService } from '../../../../shared/services/apiservice';
import { rxResource } from '@angular/core/rxjs-interop';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-select-box',
  imports: [FormsModule, ReactiveFormsModule],
  templateUrl: './select-box.html',
  styleUrl: './select-box.css',
})
export class SelectBox {
    private readonly apiService = inject(APIService);

    selectedValue = model<number>();
    route = input.required<string>();
    resourceName = input.required<string>({alias: "name"});
    displayMember = input.required<string>({alias: "display"});
    valueMember = input.required<string>({alias: "value"});
    useCache = input(true);

    resource = rxResource({
        params: () => ({
            route: this.route(),
            useCache: this.useCache()
        }),
        stream: (p) => this.apiService.getAll<any[]>(p.params.route, p.params.useCache)
    });

    datasource = computed(() => this.resource.value());
    isLoading = computed(() => this.resource.isLoading())
    error = computed(() => this.resource.error());
}
