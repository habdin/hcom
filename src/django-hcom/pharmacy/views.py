from typing import Any

from django.contrib.messages.views import SuccessMessageMixin
from django.db.models import Q
from django.http import JsonResponse
from django.urls import reverse_lazy
from django.views import generic
from django.conf import settings

from .forms import DrugForm
from .models import Drug, Company


class DrugListView(generic.ListView):
    model = Drug
    context_object_name = "objects"

    def get_context_data(self, **kwargs):
        view_as_card = self.request.GET.get("is_table", False)
        context = super().get_context_data(**kwargs)
        context["is_table"] = bool(view_as_card)
        context["title"] = "Drug List"
        context["debug"] = settings.DEBUG
        return context


class DrugSearchListView(generic.ListView):
    model = Drug
    context_object_name = "objects"
    template_name = "pharmacy/card.html"

    def get_queryset(self, search: str | None = None):
        search = self.request.GET.get("search")
        if search:
            queryset = Drug.objects.filter(
                Q(drug_name__icontains=search) | Q(company__name__icontains=search)
            )
        else:
            queryset = super().get_queryset()
        return queryset


class DrugDetailView(generic.DetailView):
    model: type[Drug] = Drug

    def get_context_data(self, **kwargs):
        context: dict[str, Any] = super().get_context_data(**kwargs)
        context["Ingredients"] = self.object.ingredient_set.all()
        return context


class DrugCreateView(SuccessMessageMixin, generic.CreateView):
    model: type[Drug] = Drug
    form_class = DrugForm
    success_url = reverse_lazy("pharmacy:drug-list")
    success_message = "Entry was successfully created."


class DrugUpdateView(SuccessMessageMixin, generic.UpdateView):
    model: type[Drug] = Drug
    form_class = DrugForm
    success_url = reverse_lazy("pharmacy:drug-list")
    success_message = "Entry was successfully updated."


class DrugDeleteView(SuccessMessageMixin, generic.DeleteView):
    model: type[Drug] = Drug
    success_url = reverse_lazy("pharmacy:drug-list")
    success_message = "Entry was successfully deleted."


def LoadData(request):
    """Load Model data as Json into a jquery DataTable and provide the server-side searching, ordering and
    filtering for the table.
    """

    # Extract request parameters
    draw = (
        request.GET.get("draw") if request.method == "GET" else request.POST.get("draw")
    )
    start = int(request.GET.get("start", 0))
    length = int(request.GET.get("length", 10))
    search_str = request.GET.get("search[value]", "")
    ordering_columns_index = int(request.GET.get("order[0][column]", 0))
    ordering_direction = str(request.GET.get("order[0][dir]", "asc"))

    # Filter data based on search string
    if search_str:
        drugs = Drug.objects.filter(
            Q(drug_name__icontains=search_str) | Q(company__name__icontains=search_str)
        )
    else:
        drugs = Drug.objects.all()

    # Get the total records before filtering
    total_Count = Drug.objects.count()

    # Get the count after filtering
    recordsFiltered = drugs.count()

    # Set the ordering_field
    ordering_field = [
        "id",
        "drug_name",
        "company",
        "drug_dose",
        "drug_unit",
        "drug_form",
        "drug_price",
    ][ordering_columns_index]

    # Determine the sorting order direction
    if ordering_direction == "desc":
        ordering_field = f"-{ordering_field}"

    drugs = drugs.order_by(ordering_field)[start : start + length]

    data = [
        {
            "id": drug.id,
            "drug_name": drug.drug_name,
            "company": drug.company.name,
            "drug_dose": drug.drug_dose,
            "drug_unit": drug.drug_unit.name,
            "drug_form": drug.drug_form.name,
            "drug_price": drug.drug_price,
        }
        for drug in drugs
    ]

    response = {
        "draw": draw,
        "data": data,
        "recordsTotal": total_Count,
        "recordsFiltered": recordsFiltered,
    }
    return JsonResponse(response)


class CompanyListView(generic.ListView):
    model = Company
    context_object_name = "objects"
    paginate_by = 10

    def get_context_data(self, **kwargs):
        view_as_table = self.request.GET.get("is_table", False)
        context = super().get_context_data(**kwargs)
        context["title"] = "Company List"
        context["is_table"] = not bool(view_as_table)
        return context
